using System;
using System.Security.Claims;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GraphQL;
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
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Data;
using HotChocolate.Types.Pagination;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Path = System.IO.Path;


var builder = WebApplication.CreateBuilder(args);

var myLoggerFactory = LoggerFactory.Create(loggingBuilder => { loggingBuilder.AddConsole(); });
var firebaseName = Guid.NewGuid().ToString();

// This method gets called by the runtime. Use this method to add services to the container.
// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(x =>
    x
        .UseLoggerFactory(myLoggerFactory)
        .EnableDetailedErrors()
        .UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// ! Controllers?
builder.Services.AddControllers();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services
    .AddAuthentication(FirebaseAuthenticationOptions.SchemeName)
    .AddScheme<FirebaseAuthenticationOptions, FirebaseAuthenticationHandler>(
        FirebaseAuthenticationOptions.SchemeName, null);

var contentRoot = builder.Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
var firebaseCredential = Path.Combine(contentRoot, "firebase-admin-sdk.json");
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(firebaseCredential),
}, builder.Environment.IsEnvironment("Testing") ? firebaseName : "[DEFAULT]");

builder.Services.AddSingleton<IIdSerializer, IdSerializer>();

builder.Services.AddHttpContextAccessor();
builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
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
    .AddAuthorization()
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
var app = builder.Build();

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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
            EnableSchemaRequests = app.Environment.IsDevelopment(),
            Tool = {Enable = app.Environment.IsDevelopment()}
        });
});

app.Run();

public partial class Program { }
