using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GraphQL.Authentication
{
    public class FirebaseAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string SchemeName = "Firebase";
    }

    public class FirebaseAuthenticationHandler : AuthenticationHandler<FirebaseAuthenticationOptions>
    {
        private readonly ILogger<FirebaseAuthenticationHandler> _logger;

        public FirebaseAuthenticationHandler(
            IOptionsMonitor<FirebaseAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<FirebaseAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing bearer request header");

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) ||
                !authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var token = authorizationHeader.Substring("bearer".Length).Trim();
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Bearer token was empty/null");

            try
            {
                return await ValidateToken(token);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return AuthenticateResult.Fail("Unauthorized");
            }
        }

        private async Task<AuthenticateResult> ValidateToken(string token)
        {
            List<Claim> claims;
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid);
                claims = new List<Claim>
                {
                    new(ClaimTypes.Sid, userRecord.Uid),
                    new(ClaimTypes.Name, userRecord.DisplayName ?? ""),
                    new(ClaimTypes.Expiration, decodedToken.ExpirationTimeSeconds.ToString()),
                    new (ClaimTypes.Email, userRecord.Email ?? ""),
                    new (ClaimTypes.GroupSid,UserGroups.User),
                    new (CustomClaimTypes.PhotoUrl, userRecord.PhotoUrl ?? "")
                };
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return AuthenticateResult.Fail("Failed to decode bearer token");
            }

            var identity = new ClaimsIdentity(claims, FirebaseAuthenticationOptions.SchemeName);
            var principal = new GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}