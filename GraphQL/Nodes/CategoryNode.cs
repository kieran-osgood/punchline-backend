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
    [ExtendObjectType(typeof(Category))]
    public class CategoryNode
    {
        [NodeResolver]
        public static Task<Category> GetCategoryAsync(
            int id,
            CategoryByIdDataLoader categoryById,
            CancellationToken cancellationToken)
            => categoryById.LoadAsync(id, cancellationToken);

        [BindMember(nameof(Category.Jokes), Replace = true)]
        public async Task<IEnumerable<Joke>> GetJokesAsync(
            [Parent] Category category,
            JokeByIdDataLoader dataLoader,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var ids = await (
                    from c in dbContext.Categories
                    where c.Id == category.Id
                    select c.Jokes.Select(x => x.Id).ToList())
                .FirstOrDefaultAsync(cancellationToken);

            return await dataLoader.LoadAsync(ids ?? new List<int>(), cancellationToken);
        }

    }
}