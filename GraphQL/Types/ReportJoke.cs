using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class JokeReportType : ObjectType<JokeReport>
    {
        protected override void Configure(IObjectTypeDescriptor<JokeReport> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<JokeReportByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.ReportingUser)
                .ResolveWith<UserResolvers>(t => t.GetUserAsync(default!, default!, default!, default!))
                .UseDbContext<ApplicationDbContext>()
                .Name("reportingUser");
            descriptor
                .Field(t => t.ReportedJoke)
                .ResolveWith<UserResolvers>(t => t.GetJokeAsync(default!, default!, default!, default!))
                .UseDbContext<ApplicationDbContext>()
                .Name("reportedJoke");
        }

        private class UserResolvers
        {
            public async Task<User> GetUserAsync(
                [Parent] JokeReport jokeReport ,
                UserByIdDataLoader dataLoader,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                var ids = await (
                        from jr in dbContext.JokeReports
                        where jr.Id == jokeReport.Id
                        select jr.ReportingUser.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                return await dataLoader.LoadAsync(ids, cancellationToken);
            }
            public async Task<Joke> GetJokeAsync(
                [Parent] JokeReport jokeReport ,
                JokeByIdDataLoader dataLoader,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                var ids = await (
                        from jr in dbContext.JokeReports
                        where jr.Id == jokeReport.Id
                        select jr.ReportedJoke.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                return await dataLoader.LoadAsync(ids, cancellationToken);
            }
        }
    }
}