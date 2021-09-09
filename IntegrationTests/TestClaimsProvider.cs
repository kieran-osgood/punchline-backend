using System;
using System.Collections.Generic;
using System.Security.Claims;
using GraphQL;
using GraphQL.Authentication;

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

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider();
            
            provider.Claims.Add(new Claim(ClaimTypes.Role, "User"));
            provider.Claims.Add(new(ClaimTypes.Sid, "n3mU54T2ZJTrS7DySfDtPf6dB9M2"));
            provider.Claims.Add(new(ClaimTypes.Name, "Test User"));
            provider.Claims.Add(new(ClaimTypes.Expiration, "99999999"));
            provider.Claims.Add(new (ClaimTypes.Email, "kieranbosgood@gmail.com"));
            provider.Claims.Add(new (ClaimTypes.GroupSid,UserGroups.User));
            provider.Claims.Add(new (CustomClaimTypes.PhotoUrl, ""));
            
            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));

            return provider;
        }
    }
}