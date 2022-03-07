using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;

using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ErrorCodes = GraphQL.Common.ErrorCodes;

namespace GraphQL.Entities.JokeReport
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class JokeReportMutations
    {
        private readonly ILogger<JokeReportMutations> _logger;

        public JokeReportMutations(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<JokeReportMutations>();
        }

        public record JokeReportInput([property: ID(nameof(Joke))] int Id, string Description);

        [Authorize]
        [UseApplicationDbContext]
        public async Task<MutateJokeReportPayload> AddJokeReport(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string userUid,
            JokeReportInput input,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                var joke = await context.Jokes.FirstOrDefaultAsync(x => x.Id == input.Id, cancellationToken);
                var user = await context.Users.FirstOrDefaultAsync(x => x.FirebaseUid == userUid, cancellationToken);

                if (joke == null || user == null) throw new Exception("User is undefined");

                var jokeReport = new Data.JokeReport()
                {
                    Description = input.Description,
                    ReportedJoke = joke,
                    ReportingUser = user,
                    CreatedAt = DateTime.Now
                };

                context.JokeReports.Add(jokeReport);

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
                await transaction.CommitAsync(cancellationToken);
                
                return new MutateJokeReportPayload(jokeReport);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateJokeReportPayload(new List<UserError>
                    {new(ErrorCodes.DuplicateEntry)});
            }
            catch (Exception e)
            {
                _logger.LogError("A {Message}", e.ToString());
                return new MutateJokeReportPayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }
    }
}