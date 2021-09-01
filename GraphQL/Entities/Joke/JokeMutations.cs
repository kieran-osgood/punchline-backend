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

namespace GraphQL.Entities.Joke
{
    [ExtendObjectType(ObjectTypes.Mutation)]
    public class JokeMutations
    {
        private readonly ILogger<JokeMutations> _logger;

        public JokeMutations(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<JokeMutations>();
        }

        [Authorize]
        [UseApplicationDbContext]
        public async Task<MutateJokePayload> ReportJoke(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string userUid,
            [ID(nameof(Data.Joke))] int id,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var joke = await context.Jokes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                var user = await context.Users.FirstOrDefaultAsync(x => x.FirebaseUid == userUid, cancellationToken);

                if (joke == null || user == null)
                {
                    return new MutateJokePayload(new List<UserError>
                        {new(ErrorCodes.ResourceNotFound)});
                }

                var userJokeHistory = new Data.UserJokeHistory
                {
                    Bookmarked = false,
                    Rating = RatingValue.Reported,
                    Joke = joke,
                    User = user,
                    CreatedAt = DateTime.Now
                };
                
                user.UserJokeHistories.Add(userJokeHistory);
                joke.ReportCount += 1;
                
                await context.SaveChangesAsync(cancellationToken);
                
                return new MutateJokePayload(joke);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateJokePayload(new List<UserError>
                    {new(ErrorCodes.DuplicateEntry)});
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateJokePayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }

    }
}