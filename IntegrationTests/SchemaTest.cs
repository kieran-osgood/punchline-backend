using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests
{
    public class SchemaTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SchemaTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Schema_Changed()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/graphql?sdl"); // formerly /graphql/schema on v10

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var schema = await response.Content.ReadAsStringAsync();
            Snapshot.Match(schema);
        }
        
        [Fact]
        public async Task Check_Something()
        {
            using var scope = _factory.Services.CreateScope();
            var dbpool = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            // var requestExecutor = scope.ServiceProvider.GetRequiredService<IRequestExecutorBuilder>();
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                BaseAddress = new Uri("http://localhost/")
            });
            var context = dbpool.CreateDbContext();
        
            var response = await client.PostAsync("graphql",
                new StringContent(@"query Jokes {
                          jokes(first: 2, where: {score: { gt: 3 }}) {
                            nodes {
                              id
                              body
                            }
                          }
                        }"));
            string query =
                @"query getPersons {
                persons {
                    nodes {
                        name
                    }
                }
            }";
            var jokes = await context.Jokes.ToListAsync();
            // jokes.ToJson().MatchSnapshot();
        }
    }
}