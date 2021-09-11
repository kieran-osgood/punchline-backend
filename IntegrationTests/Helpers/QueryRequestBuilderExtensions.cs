using System.Security.Claims;
using GraphQL;
using HotChocolate.Execution;

namespace IntegrationTests.Helpers
{
    public static class QueryRequestBuilderExtensions
    {
        public static IQueryRequestBuilder AddAuthorizedUser(this QueryRequestBuilder builder)
        {
            return builder
                .AddProperty(nameof(ClaimsPrincipal), new ClaimsPrincipal(new ClaimsIdentity("Test")))
                .AddProperty(nameof(GlobalStates.HttpContext.UserUid), Utilities.GetValidAuthUser().FirebaseUid);
        }
    }
}