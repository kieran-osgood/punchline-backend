using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType("Query")]
    public class JokeQueries
    {
        [UseApplicationDbContext]
        [Authorize]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Joke> GetJokes(
            [ScopedService] ApplicationDbContext context) =>
            context.Jokes;
    }
}