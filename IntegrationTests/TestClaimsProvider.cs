using System;
using System.Collections.Generic;
using System.Security.Claims;
using GraphQL;
using GraphQL.Authentication;
using IntegrationTests.Helpers;

namespace IntegrationTests
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            var user = Utilities.GetValidAuthUser(); 
            provider.Claims.Add(new Claim(ClaimTypes.Role, "User"));
            provider.Claims.Add(new(ClaimTypes.Sid, user.FirebaseUid));
            provider.Claims.Add(new(ClaimTypes.Name, user.Name));
            provider.Claims.Add(new(ClaimTypes.Expiration, "99999999"));
            provider.Claims.Add(new (ClaimTypes.GroupSid,UserGroups.User));
            provider.Claims.Add(new (CustomClaimTypes.PhotoUrl, ""));
            return provider;
        }
    }
}