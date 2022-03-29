using GraphQL.Data;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Nodes
{
    [Node]
    [ExtendObjectType(typeof(Joke))]
    public class JokeNode
    {
        [NodeResolver]
        public static Task<Joke> GetJokeAsync(
            int id,
            JokeByIdDataLoader jokeById,
            CancellationToken cancellationToken)
            => jokeById.LoadAsync(id, cancellationToken);

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