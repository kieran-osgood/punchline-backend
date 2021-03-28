using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Data;
using GraphQL.Entities.Joke;
using GraphQL.Types;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Pagination;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests
{
    public class SchemaTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ApplicationDbContext _context;

        public SchemaTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;

            _context = _factory.Services.GetRequiredService<ApplicationDbContext>();
           var test =   _factory.Services.GetRequiredService<IRequestExecutorBuilder>();

        }

        [Fact]
        public async Task Schema_Changed()
        {
            // arrange
            // act
            ISchema schema = await new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(
                    options => options.UseInMemoryDatabase("InMemoryDbForTesting"))
                .AddGraphQL()
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<JokeQueries>()
                .AddType<JokeType>()
                .AddFiltering()
                .AddSorting()
                .SetPagingOptions(new PagingOptions
                {
                    DefaultPageSize = 500,
                    MaxPageSize = 500,
                    IncludeTotalCount = true
                })
                .EnableRelaySupport()
                .BuildSchemaAsync();
            // var dbcontext = schema.Services?.GetService<ApplicationDbContext>();
            // Utilities.InitializeDbForTests(dbcontext);
            // var joke = dbcontext?.Jokes.FirstOrDefault(x => x.Id == 1);
            // assert
            schema.Print().MatchSnapshot();
        }

        [Fact]
        public async Task Check_Something()
        {
            var client = _factory.CreateClient();
            var jokes = await _context.Jokes.FirstOrDefaultAsync(x => x.Id == 1);
        }
    }
}