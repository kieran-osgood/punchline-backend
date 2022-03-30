using System.Security.Claims;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GraphQL.Authentication;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Entities.BugReport;
using GraphQL.Entities.Category;
using GraphQL.Entities.Joke;
using GraphQL.Entities.JokeReport;
using GraphQL.Entities.User;
using GraphQL.Entities.UserJokeHistory;
using GraphQL.Nodes;
using GraphQL.Repositories.Category;
using HotChocolate.AspNetCore;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
using Path = System.IO.Path;

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
                    .UseMySQL(_configuration.GetConnectionString("DefaultConnection")));
            
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
            }, _environment.IsEnvironment("Testing") ? _firebaseName : "[DEFAULT]");

            services.AddSingleton<IIdSerializer, IdSerializer>();

            services.AddHttpContextAccessor();
            
            services
                .AddGraphQLServer()
                .AddQueryType()
                .AddMutationType()
                .AddAuthorization()
                // .AddMutationConventions()
                .AddTypeExtension<JokeQueries>()
                .AddTypeExtension<CategoryQueries>()
                .AddTypeExtension<UserJokeHistoryQueries>()
                
                .AddTypeExtension<UserJokeHistoryMutations>()
                .AddTypeExtension<JokeMutations>()
                .AddTypeExtension<JokeReportMutations>()
                .AddTypeExtension<BugReportMutations>()
                .AddTypeExtension<UserMutations>()
                
                .AddTypeExtension<JokeNode>()
                .AddTypeExtension<BugReportNode>()
                .AddTypeExtension<JokeReportNode>()
                .AddTypeExtension<UserNode>()
                .AddTypeExtension<CategoryNode>()
                .AddTypeExtension<UserJokeHistoryNode>()
                
                .AddDataLoader<JokeByIdDataLoader>()
                .AddDataLoader<JokeReportByIdDataLoader>()
                .AddDataLoader<CategoryByIdDataLoader>()
                .AddDataLoader<UserJokeHistoryByIdDataLoader>()

                .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
                .RegisterService<IDbContextFactory<ApplicationDbContext>>(ServiceKind.Synchronized)
                .AddSorting()
                .AddFiltering()
                .AddGlobalObjectIdentification()
                .SetPagingOptions(new PagingOptions {InferConnectionNameFromField = false})
                .AddHttpRequestInterceptor((context, executor, builder, token) =>
                {
                    builder.AddProperty(GlobalStates.HttpContext.Claims, context.User);

                    var userUid = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    builder.AddProperty(GlobalStates.HttpContext.UserUid, userUid);

                    return default;
                })
                .SetPagingOptions(new PagingOptions
                {
                    DefaultPageSize = 25,
                    MaxPageSize = 500,
                    IncludeTotalCount = true
                })
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseXRay("Punchline", _configuration); // name of the app
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
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