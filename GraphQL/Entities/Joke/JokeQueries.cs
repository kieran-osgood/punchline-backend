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
        public async Task<IQueryable<Data.Joke>> GetJokes(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string? userUid,
            [Service] ICategoryRepository categoryRepository,
            JokeLength jokeLength = JokeLength.Medium)
        {
            // Casting to variable because of upstream bug: https://github.com/npgsql/efcore.pg/issues/1281
            var length = (int) jokeLength;
            var categoryIds = await categoryRepository.GetCategoryIdsByUserUid(userUid);

            // Monitor this for performance - nested NOT (EXIST) - top level join may be better
            return (from j in context.Jokes
                where j.Body.Length < length
                from c in j.Categories.Where(x => categoryIds.Count <= 0 || categoryIds.Contains(x.Id))
                where !context.UserJokeHistory.Any(x => x.JokeId == j.Id)
                select j).Distinct();
        }
    }
}