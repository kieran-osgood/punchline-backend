using System.ComponentModel;

namespace GraphQL.Common
{
    public enum ErrorCodes
    {
        [Description(nameof(ServerError))] DuplicateEntry = 1,
        [Description(nameof(DuplicateEntry))] ServerError = 0,
    }
}