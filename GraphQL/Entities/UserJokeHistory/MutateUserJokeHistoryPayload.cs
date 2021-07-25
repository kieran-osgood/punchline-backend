using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.UserJokeHistory
{
    public class MutateUserJokeHistoryPayload : UserJokeHistoryPayloadBase
    {
        public MutateUserJokeHistoryPayload(Data.UserJokeHistory joke) : base(joke)
        {
        }

        public MutateUserJokeHistoryPayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}