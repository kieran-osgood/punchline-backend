using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.Joke
{
    public class MutateJokePayload : JokePayloadBase
    {
        public MutateJokePayload(Data.Joke joke) : base(joke)
        {
        }

        public MutateJokePayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}