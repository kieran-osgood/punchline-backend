using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.User
{
    public class UserPayloadBase : Payload
    {
        protected UserPayloadBase(Data.User joke)
        {
            User = joke;
        }

        protected UserPayloadBase(IReadOnlyList<UserError> errors): base(errors)
        {
        }

        public Data.User? User { get; }
    }
}