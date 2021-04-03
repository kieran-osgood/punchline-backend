using System;
using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType("Query")]
    public class JokeQueries
    {
        public enum JokeLength
        {
            Small = 175,
            Medium = 400,
            Large = 5000
        }

        [UseApplicationDbContext]
        [Authorize]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Joke> GetJokes(
            [ScopedService] ApplicationDbContext context,
            JokeLength jokeLength)
        {
            var length = (int) jokeLength;
            return from j in context.Jokes
                where j.Body.Length < length
                select j;
        }
    }
}