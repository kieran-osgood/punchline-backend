using System;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GraphQL.Authentication;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Entities.Category;
using GraphQL.Entities.Joke;
using GraphQL.Types;
using HotChocolate.AspNetCore;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
                    .UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services
                .AddAuthentication(FirebaseAuthenticationOptions.SchemeName)
                .AddScheme<FirebaseAuthenticationOptions, FirebaseAuthenticationHandler>(
                    FirebaseAuthenticationOptions.SchemeName, null);

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(_configuration["GOOGLE_APPLICATION_CREDENTIALS"])
            });

            services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                .AddType<JokeQueries>()
                .AddType<CategoryQueries>()
                .AddType<JokeType>()
                .AddType<CategoryType>()
                .AddDataLoader<JokeByIdDataLoader>()
                .AddDataLoader<CategoryByIdDataLoader>()
                .AddSorting()
                .AddFiltering()
                .AddAuthorization()
                .SetPagingOptions(new PagingOptions
                {
                    DefaultPageSize = 25,
                    MaxPageSize = 500,
                    IncludeTotalCount = true
                })
                .EnableRelaySupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

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