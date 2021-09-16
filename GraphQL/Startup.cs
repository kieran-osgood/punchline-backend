using System;
using System.IO;
using System.Security.Claims;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GraphQL.Authentication;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Entities.Category;
using GraphQL.Entities.Joke;
using GraphQL.Entities.User;
using GraphQL.Entities.UserJokeHistory;
using GraphQL.Repositories.Category;
using static GraphQL.Static.ObjectTypes;
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

        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
        private readonly string _firebaseName = Guid.NewGuid().ToString();
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
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
            // ! Controllers?
            services.AddControllers();
            services.AddTransient<ICategoryRepository, CategoryRepository>();

            services
                .AddAuthentication(FirebaseAuthenticationOptions.SchemeName)
                .AddScheme<FirebaseAuthenticationOptions, FirebaseAuthenticationHandler>(
                    FirebaseAuthenticationOptions.SchemeName, null);
            
            var contentRoot = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var firebaseCredential = Path.Combine(contentRoot, "firebase-admin-sdk.json");
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(firebaseCredential),
            }, _environment.IsEnvironment("Testing") ?  _firebaseName : "[DEFAULT]");
            
            services.AddHttpContextAccessor();
            services
                .AddGraphQLServer()
                .SetPagingOptions(new PagingOptions {InferConnectionNameFromField = false})
                .AddHttpRequestInterceptor((context, executor, builder, token) =>
                {
                    builder.AddProperty(GlobalStates.HttpContext.Claims, context.User);

                    var userUid = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    builder.AddProperty(GlobalStates.HttpContext.UserUid, userUid);

                    return default;
                })
                .AddQueryType(d => d.Name(Query))
                .AddType<JokeQueries>()
                .AddType<CategoryQueries>()
                .AddType<UserJokeHistoryQueries>()
                .AddMutationType(d => d.Name(Mutation))
                .AddType<UserJokeHistoryMutations>()
                .AddType<UserMutations>()
                .AddType<JokeType>()
                .AddType<CategoryType>()
                .AddType<UserJokeHistoryType>()
                .AddDataLoader<JokeByIdDataLoader>()
                .AddDataLoader<CategoryByIdDataLoader>()
                .AddDataLoader<UserJokeHistoryByIdDataLoader>()
                .AddSorting()
                .AddFiltering()
                .AddAuthorization()
                .SetPagingOptions(new PagingOptions
                {
                    DefaultPageSize = 25,
                    MaxPageSize = 500,
                    IncludeTotalCount = true
                })
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL("/graphql")
                    .WithOptions(new GraphQLServerOptions()
                    {
                        EnableSchemaRequests = env.IsDevelopment(),
                        Tool = {Enable = env.IsDevelopment()}
                    });
            });
        }
    }
}