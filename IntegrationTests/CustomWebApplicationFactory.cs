using System;
using System.Linq;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Entities.Joke;
using GraphQL.Types;
using HotChocolate.Execution;
using HotChocolate.Types.Pagination;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });


                services
                    .AddGraphQL()
                    .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<JokeQueries>()
                    .AddType<JokeType>()
                    .AddType<JokeByIdDataLoader>()
                    .AddType<Category>()
                    .AddType<CategoryByIdDataLoader>()
                    .AddFiltering()
                    .AddSorting()
                    .SetPagingOptions(new PagingOptions
                    {
                        DefaultPageSize = 500,
                        MaxPageSize = 500,
                        IncludeTotalCount = true
                    })
                    .EnableRelaySupport()
                    ;
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    var context = db.CreateDbContext();
                    context.Database.EnsureCreated();

                    try
                    {
                        Utilities.InitializeDbForTests(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}