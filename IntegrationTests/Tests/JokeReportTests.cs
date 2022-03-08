
using System.Text.Json;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using GraphQL.Entities.JokeReport;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types.Relay;
using IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class JokeReportTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task Add_Jokes_Report_Commits()
        {
            var factory =
                new CustomWebApplicationFactory<Program>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            var user = Utilities.GetValidAuthUser();
            var joke = Utilities.GetValidJoke();
            const string description = "This joke was despicable!!!!!!!";
            var idSerializer = factory.Services.GetRequiredService<IIdSerializer>();
            var jokeId = idSerializer.Serialize("", nameof(Joke), joke.Id);

            var query = 
$@"mutation RateJoke {{
    addJokeReport(input: {{
        id: {"\""  + jokeId + "\""},
        description: {"\"" + description+ "\""}
    }}) {{
        jokeReport {{
            id
            reportingUser {{
                id
                name
            }}
            reportedJoke {{
                    id
                    title
                }}
        }}
        errors {{
            code
            message
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