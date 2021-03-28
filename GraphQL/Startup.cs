using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Entities.Joke;
using GraphQL.Types;
using HotChocolate.AspNetCore;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GraphQL
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });


        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<ApplicationDbContext>(x =>
                x
                    .UseLoggerFactory(MyLoggerFactory)
                    .EnableDetailedErrors()
                    .UseNpgsql(
                        _configuration.GetConnectionString("DefaultConnection"),
                        options => { options.MigrationsHistoryTable("__ArMigrationsHistory"); }));
            services.AddControllers();

            services.AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                .AddType<JokeQueries>()

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
                .AddDataLoader<JokeByIdDataLoader>()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL("/graphql")
                    .WithOptions(new GraphQLServerOptions()
                    {
                        EnableSchemaRequests = _environment.IsDevelopment(),
                        Tool = {Enable = _environment.IsDevelopment()}
                    });
            });
        }
    }
}