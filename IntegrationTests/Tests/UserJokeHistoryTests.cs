using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using GraphQL.Entities.Joke;
using GraphQL.Entities.UserJokeHistory;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types.Relay;
using IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class UserJokeHistoryTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Theory]
        [InlineData(RatingValue.Good)]
        // [InlineData(RatingValue.Reported)]
        // [InlineData(RatingValue.Bad)]
        // [InlineData(RatingValue.Skip)]
        public async Task Updates_Type_Of_Rating(RatingValue rating)
        {
            var factory =
                new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            // const int id = 1;
            // var idSerializer = factory.Services.GetRequiredService<IIdSerializer>();
            // var jokeId = idSerializer.Serialize("", nameof(Joke), id);

            var input = new UserJokeHistoryMutations.RateJokeInput(1, rating, false);
            
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
            var jsonString = await result.ToJsonAsync();
            var foo = Newtonsoft.Json.JsonConvert.DeserializeObject<MutateUserJokeHistoryPayload>(jsonString);
            Console.WriteLine(foo.UserJokeHistory.Id);
            // Snapshot.Match(await result.ToJsonAsync(), $"{nameof(Jokes_Length_Filters_Results)}_{length}");
            // Assert the field 'Street' of the 'Address' of the person

Snapshot.Match(jsonString, matchOption => matchOption.Assert(
                    fieldOption => Assert.Equal("Sm9rZVJlcG9ydAppMQ==", fieldOption.Field<string>("Data.AddJokeReport.JokeReport.Id"))));

// // Asserts the field 'Code' of the field 'Country' of the 'Address' of the person
// Snapshot.Match<Person>(person, matchOption => matchOption.Assert(
//                     fieldOption => Assert.Equal("De", fieldOption.Field<CountryCode>("Address.Country.Code"))));

// // Asserts the fist 'Id' field of the 'Relatives' array of the person
// Snapshot.Match<Person>(person, > matchOption.Assert(
//                     fieldOption => Assert.NotNull(fieldOption.Field<string>("Relatives[0].Id"))));

// // Asserts every 'Id' field of all the 'Relatives' of the person
// Snapshot.Match<Person>(person, > matchOption.Assert(
//                     fieldOption => Assert.NotNull(fieldOption.Fields<string>("Relatives[*].Id"))));
 
// // Asserts 'Relatives' array is not empty
// Snapshot.Match<Person>(person, > matchOption.Assert(
//                     fieldOption => Assert.NotNull(fieldOption.Fields<TestPerson>("Relatives[*]"))));


        }
    }
}