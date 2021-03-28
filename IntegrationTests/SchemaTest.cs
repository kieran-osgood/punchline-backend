using System;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            // arrange
            // act
            // ISchema schema = await new ServiceCollection()
            //     .AddDbContext<ApplicationDbContext>(
            //         options => options.UseInMemoryDatabase("InMemoryDbForTesting"))
            //     .AddGraphQL()
            //     .AddQueryType(d => d.Name("Query"))
            //     .AddTypeExtension<JokeQueries>()
            //     .AddType<JokeType>()
            //     .AddFiltering()
            //     .AddSorting()
            //     .SetPagingOptions(new PagingOptions
            //     {
            //         DefaultPageSize = 500,
            //         MaxPageSize = 500,
            //         IncludeTotalCount = true
            //     })
            //     .EnableRelaySupport()
            //     .BuildSchemaAsync();
            // // var dbcontext = schema.Services?.GetService<ApplicationDbContext>();
            // // Utilities.InitializeDbForTests(dbcontext);
            // // var joke = dbcontext?.Jokes.FirstOrDefault(x => x.Id == 1);
            // // assert
            // schema.Print().MatchSnapshot();
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
                new StringContent(@"
query Jokes {
  jokes(first: 2, where: {score: { gt: 3 }}) {
    nodes {
      id
      body
    }
  }
}
"));
            var jokes = await context.Jokes.ToListAsync();
            // jokes.ToJson().MatchSnapshot();
        }
    }
}