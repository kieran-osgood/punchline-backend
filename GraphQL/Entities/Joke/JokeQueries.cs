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
        public IQueryable<Data.Joke> GetJokes(
            [ScopedService] ApplicationDbContext context) =>
            context.Jokes.OrderBy(t => t.Id);

        [UseApplicationDbContext]
        [UsePaging]
        public IQueryable<Data.Joke> GetMoreJokes(
            [ScopedService] ApplicationDbContext context) =>
            context.Jokes.OrderBy(t => t.Id);
    }
}