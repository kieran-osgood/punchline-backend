using System;
using GraphQL.Extensions;

namespace GraphQL.Common
{
    public class UserError
    {
        public UserError(ErrorCodes code)
        {
            Message = code.AsString();
            Code = code;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public ErrorCodes Code { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string Message { get; }
    }
}