using System.ComponentModel;

namespace GraphQL.Common
{
    public enum ErrorCodes
    {
        [Description(
            "Unknown Server Error - Please refresh and contact your system administator if the issues persists")]
        ServerError = 0,

        [Description("Error updating database - duplicate record exists")]
        DuplicateEntry = 1,

        [Description("Couldn't locate the resource requested")]
        ResourceNotFound = 2,
    }
}