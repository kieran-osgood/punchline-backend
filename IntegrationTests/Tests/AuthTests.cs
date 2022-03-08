using System.Threading.Tasks;
using GraphQL;
using HotChocolate;
using HotChocolate.Execution;
using IntegrationTests.Helpers;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class AuthTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Theory]
        [InlineData(@"query Jokes {jokes(input: {jokeLengths: []}) {nodes {id}}}")]
        [InlineData(@"query Category {categories {nodes {id}}}")]
        [InlineData(@"query GetUserCategories {categories {nodes {id}}}")]
        [InlineData(@"query UserJokeHistoryBookmarks {userJokeHistoryByUserId {nodes {id}}}")]
        public async Task Unauthenticated_Request_On_Authorized_Endpoint_Denies_Access(string query)
        {
            // Setups a factory which has an AuthenticationScheme and Handler setup
            var factory = new CustomWebApplicationFactory<Program>().WithAuthentication(TestClaimsProvider.WithUserClaims()); 
            var request =
                QueryRequestBuilder
                    .New()
                    // Do not add .AddAuthorizedUser so we can test for the error cases
                    .SetQuery(query)
                    .Create();

            var executor = await factory.Services.GetRequestExecutorAsync();
            var result = await executor.ExecuteAsync(request);

            Assert.NotNull(result.Errors);
            
            var readOnlyDictionary = result.Errors[0].Extensions;
            if (readOnlyDictionary != null)
                Assert.Equal("AUTH_NOT_AUTHENTICATED", readOnlyDictionary["code"]);
        }
    }
}