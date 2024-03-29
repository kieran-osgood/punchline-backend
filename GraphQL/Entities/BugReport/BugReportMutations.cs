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

namespace GraphQL.Entities.BugReport
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class BugReportMutations
    {
        private readonly ILogger<BugReportMutations> _logger;

        public BugReportMutations(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<BugReportMutations>();
        }

        public record BugReportInput(string Description);

        [Authorize]
        
        public async Task<MutateBugReportPayload> AddBugReport(
            [ScopedService] ApplicationDbContext context,
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string userUid,
            BugReportInput input,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                var user = await context.Users.FirstOrDefaultAsync(x => x.FirebaseUid == userUid, cancellationToken);

                if (user == null) return new MutateBugReportPayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});

                var bugReport = new Data.BugReport()
                {
                    Description = input.Description,
                    ReportingUser = user,
                };

                context.BugReports.Add(bugReport);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                
                return new MutateBugReportPayload(bugReport);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new MutateBugReportPayload(new List<UserError>
                    {new(ErrorCodes.DuplicateEntry)});
            }
            catch (Exception e)
            {
                _logger.LogError("A {Message}", e.ToString());
                return new MutateBugReportPayload(new List<UserError>
                    {new(ErrorCodes.ServerError)});
            }
        }
    }
}