using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IQueryable<Data.Joke>> GetJokes(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(Startup.GlobalStates.HttpIdentityUser.UserId)] string? userId,
            JokeLength jokeLength = JokeLength.Medium)
        {
            var length = (int) jokeLength;

            var categoryIds = await
                (from u in context.Users
                    from c in u.Categories
                    where u.FirebaseUid == userId
                    select c.Id).ToListAsync();

            if (categoryIds != null && categoryIds.Count > 0)
            {
                return from j in context.Jokes
                    from c in j.Categories.Where(x => categoryIds.Contains(x.Id))
                    where j.Body.Length < length
                    select j;
            }

            return from j in context.Jokes
                where j.Body.Length < length
                select j;
        }
    }
}