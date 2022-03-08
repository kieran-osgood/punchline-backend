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
    public class JokeTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Theory]
        [InlineData(JokeLength.Small)]
        [InlineData(JokeLength.Medium)]
        [InlineData(JokeLength.Large)]
        public async Task Jokes_Length_Filters_Results(JokeLength length)
        {
            var factory =
                new CustomWebApplicationFactory<Program>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            var input = new JokeQueries.JokeQueryInput(null, null, new[] {length}, false);
            
            var query =
                @"query Jokes($input: JokeQueryInput!) {
                  jokes(input: $input) {
                    nodes {
                      id
                      body
                      length
                    }
                  }
                }";

            var executor = await factory.Services.GetRequestExecutorAsync();
            var request = QueryRequestBuilder
                .New()
                .AddAuthorizedUser()
                .SetQuery(query)
                .SetVariableValue("input", input)
                .Create();

            var result = await executor.ExecuteAsync(request);

            Assert.Null(result.Errors);
            Snapshot.Match(await result.ToJsonAsync(), $"{nameof(Jokes_Length_Filters_Results)}_{length}");
        }
    }
}