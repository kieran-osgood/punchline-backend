using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Repositories.Category;
using GraphQL.Static;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType(ObjectTypes.Query)]
    public class JokeQueries
    {
        // [Authorize]
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Joke> GetJokes(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string? userUid,
            [Service] ICategoryRepository categoryRepository,
            JokeLength jokeLength = JokeLength.Medium, 
            [ID(nameof(Data.Category))] List< int> blockedCategoryIds = default!)
        {
            // Casting to variable because of upstream bug: https://github.com/npgsql/efcore.pg/issues/1281
            var length = (int) jokeLength;
            
            // TODO Monitor this for performance - nested NOT (EXIST) - top level join may be better
            return (from j in context.Jokes
                where j.Body.Length < length
                from c in j.Categories.Where(x => blockedCategoryIds.Count <= 0 || !blockedCategoryIds.Contains(x.Id))
                where !context.UserJokeHistory.Any(x => x.JokeId == j.Id)
                select j).Distinct();
        }
    }
}