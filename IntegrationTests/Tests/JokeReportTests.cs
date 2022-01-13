using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Types.Relay;
using IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class JokeReportTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task Add_Jokes_Report_Commits()
        {
            var factory =
                new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            var joke = Utilities.GetValidJoke();
            const string description = "This joke was despicable!!!!!!!";
            var idSerializer = factory.Services.GetRequiredService<IIdSerializer>();
            var jokeId = idSerializer.Serialize("", nameof(Joke), joke.Id);

            var query =
$@"mutation RateJoke {{
    addJokeReport(input: {{
        id: {"\"" + jokeId + "\""},
        description: {"\"" + description + "\""}
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
// https://github.com/SwissLife-OSS/snapshooter
            // Want to serialize this result into a strongly typed object
            var jsonString = await result.ToJsonAsync();

            var bar = JsonConvert.DeserializeObject<dynamic>(jsonString);
            Assert.True(bar.data.addJokeReport.jokeReport.id == "Sm9rZVJlcG9ydAppMQ==");
            
            var foo = JsonConvert.DeserializeObject<dynamic>(jsonString);
            Assert.True(foo.data.addJokeReport.jokeReport.id == "Sm9rZVJlcG9ydAppMQ==");

Snapshot.Match(jsonString, matchOption => matchOption.Assert(
                    fieldOption => Assert.Equal("Sm9rZVJlcG9ydAppMQ==", fieldOption.Field<string>("data.addJokeReport.jokeReport.id"))));
Snapshot.Match(jsonString, matchOption => matchOption.Assert(
                    fieldOption => Assert.Equal("Sm9rZVJlcG9ydAppMQ==", fieldOption.Field<string>("Data.AddJokeReport.JokeReport.Id"))));
            Assert.Null(result.Errors);
            (await result.ToJsonAsync()).MatchSnapshot();
        }
    }

    namespace GraphQLCodeGen
    {
        public class RateJokeGQL
        {
            /// <summary>
            /// RateJokeGQL.Request 
            /// </summary>
            public static GraphQLRequest Request()
            {
                return new GraphQLRequest
                {
                    Query = RateJokeDocument,
                    OperationName = "RateJoke"
                };
            }

            /// <remarks>This method is obsolete. Use Request instead.</remarks>
            public static GraphQLRequest getRateJokeGQL()
            {
                return Request();
            }

            public static string RateJokeDocument = @"
        mutation RateJoke {
          addJokeReport(input: {id: ""abc"", description: ""123""}) {
            jokeReport {
              id
              reportingUser {
                id
                name
              }
              reportedJoke {
                id
                title
              }
            }
            errors {
              code
              message
            }
          }
        }
        ";
        }
    }
}