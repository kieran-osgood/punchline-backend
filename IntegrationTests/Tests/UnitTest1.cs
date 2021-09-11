using System.Threading.Tasks;
using GraphQL;
using HotChocolate;
using HotChocolate.Execution;
using IntegrationTests.Helpers;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task GetJokes()
        {
            var factory = new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithAdminClaims());; 
            const string query = @"query Jokes {
                  jokes(jokeLength: SMALL) {
                    nodes {
                      id
                      body
                    }
                  }
                }";

            var executor = await factory.Services.GetRequestExecutorAsync();
            var request = QueryRequestBuilder
                .New()
                .AddAuthorisedUser()
                .SetQuery(query)
                .Create();
            var result = await executor.ExecuteAsync(request);

            Assert.Null(result.Errors);
            (await result.ToJsonAsync()).MatchSnapshot();
        }


        [Theory]
        [InlineData(@"query Jokes {
                  jokes {
                    nodes {
                      id
                      body
                    }
                  }
                }")]
        public async Task Unauthenticated_User_Request(string query)
        {
            var factory = new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithAdminClaims());; 
            var request =
                QueryRequestBuilder
                    .New()
                    .SetQuery(query)
                    .Create();

            var executor = await factory.Services.GetRequestExecutorAsync();
            var result = await executor.ExecuteAsync(request);

            (await result.ToJsonAsync()).MatchSnapshot();
        }
    }
}