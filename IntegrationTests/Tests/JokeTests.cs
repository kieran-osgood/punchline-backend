using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using GraphQL.Entities.Joke;
using HotChocolate;
using HotChocolate.Execution;
using IntegrationTests.Helpers;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class JokeTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Theory]
        [InlineData(JokeLength.Small)]
        [InlineData(JokeLength.Medium)]
        [InlineData(JokeLength.Large)]
        public async Task Jokes_Length_Filters_Results(JokeLength length)
        {
            var factory = new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());; 
            var query = 
                $@"query Jokes {{
                  jokes(jokeLength: {length.ToString().ToUpper()}) {{
                    nodes {{
                      id
                      body
                    }}
                  }}
                }}";

            var executor = await factory.Services.GetRequestExecutorAsync();
            var request = QueryRequestBuilder
                .New()
                .AddAuthorizedUser()
                .SetQuery(query)
                .Create();
            
            var result = await executor.ExecuteAsync(request);

            Assert.Null(result.Errors);
            
            (await result.ToJsonAsync()).MatchSnapshot();
        }
    }
}