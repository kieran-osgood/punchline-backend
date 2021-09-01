using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.User
{
    public class UserPayload : UserPayloadBase
    {
        public UserPayload(Data.User user) : base(user)
        {
        }

        public UserPayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}