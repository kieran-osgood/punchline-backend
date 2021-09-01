using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.UserJokeHistory
{
    public class UserJokeHistoryPayloadBase : Payload
    {
        protected UserJokeHistoryPayloadBase(Data.UserJokeHistory joke)
        {
            UserJokeHistory = joke;
        }

        protected UserJokeHistoryPayloadBase(IReadOnlyList<UserError> errors): base(errors)
        {
        }

        public Data.UserJokeHistory? UserJokeHistory { get; }
    }
}