using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.BugReport
{
    public class MutateBugReportPayload : BugReportPayloadBase
    {
        public MutateBugReportPayload(Data.BugReport bugReport) : base(bugReport)
        {
        }

        public MutateBugReportPayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}