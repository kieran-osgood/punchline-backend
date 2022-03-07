using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Nodes
{
    [Node]
    [ExtendObjectType(typeof(JokeReport))]
    public class JokeReportNode : ObjectType<JokeReport>
    {
        [BindMember(nameof(JokeReport.ReportingUser), Replace = true)]
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

        [BindMember(nameof(JokeReport.ReportedJoke), Replace = true)]
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