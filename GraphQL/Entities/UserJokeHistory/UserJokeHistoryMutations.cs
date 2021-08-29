using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Static;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ErrorCodes = GraphQL.Common.ErrorCodes;

namespace GraphQL.Entities.UserJokeHistory
{
    [ExtendObjectType(ObjectTypes.Mutation)]
    public class UserJokeHistoryMutations
    {
        private readonly ILogger<UserJokeHistoryMutations> _logger;

        public UserJokeHistoryMutations(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<UserJokeHistoryMutations>();
        }


        [Authorize]
        [UseApplicationDbContext]
        public async Task<MutateUserJokeHistoryPayload> RateJoke(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string userUid,
            RateJokeInput input,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var joke = await context.Jokes.FirstOrDefaultAsync(x => x.Id == input.JokeId, cancellationToken);
                var user = await context.Users.FirstOrDefaultAsync(x => x.FirebaseUid == userUid, cancellationToken);

                if (joke == null || user == null)
                {
                    return new MutateUserJokeHistoryPayload(new List<UserError>
                        {new(ErrorCodes.ResourceNotFound)});
                }

                var userJokeHistory = new Data.UserJokeHistory
                {
                    Bookmarked = input.Bookmarked,
                    Rating = input.Rating,
                    Joke = joke,
                    User = user,
                };
                user.UserJokeHistories.Add(userJokeHistory);
                await context.SaveChangesAsync(cancellationToken);
                return new MutateUserJokeHistoryPayload(userJokeHistory);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateUserJokeHistoryPayload(new List<UserError>
                    {new(ErrorCodes.DuplicateEntry)});
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateUserJokeHistoryPayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }


        [Authorize]
        [UseApplicationDbContext]
        public async Task<MutateUserJokeHistoryPayload> DeleteBookmark(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string userUid,
            [ID(nameof(Data.UserJokeHistory))] int id,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var userJokeHistory =
                    await context.UserJokeHistory.Include(x=>x.User).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (userJokeHistory == null)
                {
                    return new MutateUserJokeHistoryPayload(new List<UserError>
                        {new(ErrorCodes.ResourceNotFound)});
                }

                if (userJokeHistory.User.FirebaseUid != userUid)
                {
                    return new MutateUserJokeHistoryPayload(new List<UserError>
                        {new(ErrorCodes.NotAuthorized)});
                }

                context.UserJokeHistory.Remove(userJokeHistory);

                await context.SaveChangesAsync(cancellationToken);
                return new MutateUserJokeHistoryPayload(userJokeHistory);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateUserJokeHistoryPayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }

        public record UpdateBookmarkInput([ID(nameof(Data.UserJokeHistory))] int Id, RatingValue? Rating,
            bool? Bookmarked = false);

        [Authorize]
        [UseApplicationDbContext]
        public async Task<MutateUserJokeHistoryPayload> UpdateUserJokeHistory(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string userUid,
            UpdateBookmarkInput input,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var userJokeHistory =
                    await context.UserJokeHistory.Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.Id == input.Id, cancellationToken);

                if (userJokeHistory == null)
                {
                    return new MutateUserJokeHistoryPayload(new List<UserError>
                        {new(ErrorCodes.ResourceNotFound)});
                }

                if (userJokeHistory.User.FirebaseUid != userUid)
                {
                    return new MutateUserJokeHistoryPayload(new List<UserError>
                        {new(ErrorCodes.NotAuthorized)});
                }
                
                if (input.Rating is not null) userJokeHistory.Rating = (RatingValue) input.Rating;
                if (input.Bookmarked != null) userJokeHistory.Bookmarked= (bool) input.Bookmarked;

                await context.SaveChangesAsync(cancellationToken);
                return new MutateUserJokeHistoryPayload(userJokeHistory);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateUserJokeHistoryPayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }
    }
}