using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
        public record JokeQueryInput(
            [property: ID(nameof(Joke))] int? DeepLinkedJokeId,
            [property: ID(nameof(Data.Category))] List<int>? BlockedCategoryIds = default!,
            JokeLength[] JokeLengths = default!,
            bool ProfanityFilter = true);

        [Authorize]
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Data.Joke>> GetJokes(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string? userUid,
            JokeQueryInput input)
        {
            // Casting to variable because of upstream bug: https://github.com/npgsql/efcore.pg/issues/1281
            var user = await (from u in context.Users where u.FirebaseUid == userUid select u).FirstOrDefaultAsync();
            if (user is null)
            {
                throw new Exception("User is null");
            }

            var jokes = (from j in context.Jokes
                where input.JokeLengths.Contains(j.Length)
                where !context.UserJokeHistory.Where(x => x.UserId == user.Id).Any(x => x.JokeId == j.Id)
                select j);

            if (input.BlockedCategoryIds != null)
            {
                jokes = (from j in jokes
                    from c in j.Categories.Where(x =>
                        input.BlockedCategoryIds != null &&
                        (input.BlockedCategoryIds.Count <= 0 || !input.BlockedCategoryIds.Contains(x.Id)))
                    select j);
            }

            if (input.DeepLinkedJokeId.HasValue)
            {
                var deepLinkedJokeQueryable = from j in context.Jokes
                    where j.Id == input.DeepLinkedJokeId
                    select j;
                jokes = deepLinkedJokeQueryable.Concat(jokes).Distinct().OrderBy(x => x.Id != input.DeepLinkedJokeId);
            }

            if (input.ProfanityFilter)
            {
                jokes = from j in jokes where j.ExplicitContent == false select j;
            }
            
            return jokes;
        }
    }
}