using System.Threading.Tasks;
using GraphQL;
using HotChocolate;
using HotChocolate.Execution;
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
            var executor = await _factory.Services.GetRequestExecutorAsync();
            var query =
                @"query Jokes {
                  jokes(jokeLength: SMALL) {
                    nodes {
                      id
                      body
                    }
                  }
                }";
            var request = QueryRequestBuilder.New().SetQuery(query).Create();
            var results = await executor.ExecuteAsync(request);
            (await results.ToJsonAsync()).MatchSnapshot();
        }
    }
}