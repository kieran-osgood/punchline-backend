using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType(Name = "Query")]
    public class JokeQueries
    {
        [UseApplicationDbContext]
        [UsePaging]
        public IQueryable<Data.Joke> GetAddresses(
            [ScopedService] ApplicationDbContext context) =>
            context.Jokes.OrderBy(t => t.Id);
    }
}