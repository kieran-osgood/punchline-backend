using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.UserJokeHistory
{
    public class RateJokePayload : UserJokeHistoryPayloadBase
    {
        public RateJokePayload(Data.UserJokeHistory joke) : base(joke)
        {
        }

        public RateJokePayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}