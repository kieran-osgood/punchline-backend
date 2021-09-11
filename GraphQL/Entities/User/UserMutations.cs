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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ErrorCodes = GraphQL.Common.ErrorCodes;

namespace GraphQL.Entities.User
{
    [ExtendObjectType(ObjectTypes.Mutation)]
    public class UserMutations
    {
        private readonly ILogger<UserMutations> _logger;

        public UserMutations(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<UserMutations>();
        }


        [UseApplicationDbContext]
        public async Task<UserPayload> Login(
            [ScopedService] ApplicationDbContext context,
            UserLoginInput input,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.FirebaseUid == input.FirebaseUid,
                    cancellationToken);

                if (user is null)
                {
                    user = new Data.User
                    {
                        Name = input.Username,
                        FirebaseUid = input.FirebaseUid,
                    };
                    await context.Users.AddAsync(user, cancellationToken);
                }

                user.LastLogin = DateTime.Now;
                await context.SaveChangesAsync(cancellationToken);
                return new UserPayload(user);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new UserPayload(new List<UserError> {new(ErrorCodes.DuplicateEntry)});
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new UserPayload(new List<UserError> {new(ErrorCodes.ServerError)});
            }
        }

        [Authorize]
        [UseApplicationDbContext]
        public async Task<UserPayload> CompleteOnboarding(
            [ScopedService] ApplicationDbContext context,
            UserLoginInput input,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.FirebaseUid == input.FirebaseUid,
                    cancellationToken);
        
                if (user is null) 
                    throw new Exception($"Unable to locate user: {input.FirebaseUid}");
        
                user.OnboardingComplete = true;
                
                await context.SaveChangesAsync(cancellationToken);
                return new UserPayload(user);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.ToString());
                return new UserPayload(new List<UserError> {new(ErrorCodes.ServerError)});
            }
        }
    }
}