
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
    public class BugReportTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task Add_Bug_Report_Commits()
        {
            var factory =
                new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            const string description = "This joke was despicable!!!!!!!";

            var query = 
$@"mutation AddBugReport {{
    addBugReport(input: {{
        description: {"\"" + description+ "\""}
    }}) {{
    bugReport {{
        id
        description
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