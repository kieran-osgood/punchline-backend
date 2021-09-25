using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.Joke
{
    public class JokePayloadBase : Payload
    {
        protected JokePayloadBase(Data.Joke joke)
        {
            Joke = joke;
        }

        protected JokePayloadBase(IReadOnlyList<UserError> errors): base(errors)
        {
        }

        public Data.Joke? Joke { get; }
    }
}