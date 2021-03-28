using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType(Name = "Query")]
    public class JokeQueries
    {
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Joke> GetJokes(
            [ScopedService] ApplicationDbContext context) =>
            context.Jokes;

    }
}