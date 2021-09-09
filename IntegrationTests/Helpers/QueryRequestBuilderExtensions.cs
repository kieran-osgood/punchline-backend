using System.Security.Claims;
using GraphQL;
using HotChocolate.Execution;

namespace IntegrationTests.Helpers
{
    public static class QueryRequestBuilderExtensions
    {
        public static IQueryRequestBuilder AddAuthorisedUser(this QueryRequestBuilder builder)
        {
            return builder
                .AddProperty(nameof(ClaimsPrincipal), new ClaimsPrincipal(new ClaimsIdentity("Test")))
                .AddProperty(nameof(GlobalStates.HttpContext.UserUid), "n3mU54T2ZJTrS7DySfDtPf6dB9M2");
        }
    }
}