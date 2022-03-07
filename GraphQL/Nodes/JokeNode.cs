using System.Collections.Generic;
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
    [ExtendObjectType(typeof(Joke))]
    public class JokeNode
    {
        [NodeResolver]
        [BindMember(nameof(Joke.Categories), Replace = true)]
        public async Task<IEnumerable<Category>> GetCategoriesAsync(
            [Parent] Joke joke,
            CategoryByIdDataLoader dataLoader,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var ids = await (
                    from j in dbContext.Jokes
                    where j.Id == joke.Id
                    select j.Categories.Select(x => x.Id).ToList())
                .FirstOrDefaultAsync(cancellationToken);

            return await dataLoader.LoadAsync(ids ??  new List<int>(), cancellationToken);
        }
    }
}