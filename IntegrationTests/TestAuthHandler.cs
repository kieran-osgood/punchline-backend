using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Authentication;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var user = Utilities.GetValidAuthUser();
            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, user.FirebaseUid),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Expiration, "99999999"),
                new(ClaimTypes.GroupSid, UserGroups.User),
                new(CustomClaimTypes.PhotoUrl, "")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}