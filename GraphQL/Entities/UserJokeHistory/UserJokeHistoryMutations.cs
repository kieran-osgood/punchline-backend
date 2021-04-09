using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Static;
using HotChocolate;
using HotChocolate.Types;
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


        // [Authorize]
        [UseApplicationDbContext]
        public async Task<RateJokePayload> RateJoke(
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
                    return new RateJokePayload(new List<UserError>
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
                return new RateJokePayload(userJokeHistory);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new RateJokePayload(new List<UserError>
                    {new(ErrorCodes.DuplicateEntry)});
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new RateJokePayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }
    }
}