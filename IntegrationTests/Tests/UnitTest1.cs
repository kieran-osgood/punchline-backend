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
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public UnitTest1(CustomWebApplicationFactory<Startup> factory) => _factory = factory;

        [Fact]
        public async Task GetJokes()
        {
            var factory = _factory.WithAuthentication(TestClaimsProvider.WithAdminClaims());
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
            var request =
                QueryRequestBuilder
                    .New()
                    .SetQuery(query)
                    .Create();

            var executor = await _factory.Services.GetRequestExecutorAsync();
            var result = await executor.ExecuteAsync(request);

            (await result.ToJsonAsync()).MatchSnapshot();
        }
    }
}