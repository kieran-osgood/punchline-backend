using System;

namespace GraphQL.Common
{
    public record UserError(ErrorMessages Message, ErrorCodes Code);
}