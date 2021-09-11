using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Static;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType(ObjectTypes.Query)]
    public class JokeQueries
    {
        [Authorize]
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Data.Joke>> GetJokes(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string? userUid,
            [ID(nameof(Data.Joke))] int? deepLinkedJokeId,
            JokeLength jokeLength = JokeLength.Medium,
            [ID(nameof(Data.Category))] List<int>? blockedCategoryIds = default!)
        {
            // Casting to variable because of upstream bug: https://github.com/npgsql/efcore.pg/issues/1281
            var length = (int) jokeLength;

            var user = await (from u in context.Users where u.FirebaseUid == userUid select u).FirstOrDefaultAsync();
            if (user is null)
            {
                throw new Exception("Oops");
            }

            var jokes = (from j in context.Jokes
                where j.Body.Length <= length
                where !context.UserJokeHistory.Where(x => x.UserId == user.Id).Any(x => x.JokeId == j.Id)
                select j);

            if (blockedCategoryIds != null)
            {
                jokes = (from j in jokes
                    from c in j.Categories.Where(x =>
                        blockedCategoryIds != null &&
                        (blockedCategoryIds.Count <= 0 || !blockedCategoryIds.Contains(x.Id)))
                    select j);
            }

            if (!deepLinkedJokeId.HasValue) return jokes;

            var deepLinkedJokeQueryable = from j in context.Jokes
                where j.Id == deepLinkedJokeId
                select j;
            
            return deepLinkedJokeQueryable.Concat(jokes).Distinct().OrderBy(x => x.Id != deepLinkedJokeId);
        }
    }
}