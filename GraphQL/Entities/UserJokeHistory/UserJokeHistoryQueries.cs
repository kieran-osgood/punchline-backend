using System.Collections.Generic;
using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Static;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;

namespace GraphQL.Entities.UserJokeHistory
{
    [ExtendObjectType(ObjectTypes.Query)]
    public class UserJokeHistoryQueries
    {
        [Authorize]
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.UserJokeHistory> GetUserJokeHistories(
            [ScopedService] ApplicationDbContext context)
        {
            return from ujh in context.UserJokeHistory select ujh;
        }

        // [Authorize]
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.UserJokeHistory> GetUserJokeHistoryByUserId(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string? userUid)
        {
            return
                from ujh in context.UserJokeHistory
                from u in context.Users
                where
                    u.FirebaseUid == userUid &
                    u.Id == ujh.UserId &
                    ujh.Rating != RatingValue.Reported
                select ujh;
        }
    }
}
