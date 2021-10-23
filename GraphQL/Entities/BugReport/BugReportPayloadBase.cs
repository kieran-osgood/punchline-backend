using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.BugReport
{
    public class BugReportPayloadBase : Payload
    {
        protected BugReportPayloadBase(Data.BugReport bugReport)
        {
            BugReport = bugReport;
        }

        protected BugReportPayloadBase(IReadOnlyList<UserError> errors): base(errors)
        {
        }

        public Data.BugReport? BugReport { get; }
    }
}