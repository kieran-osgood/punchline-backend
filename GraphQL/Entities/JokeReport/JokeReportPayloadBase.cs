using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.JokeReport
{
    public class JokeReportPayloadBase : Payload
    {
        protected JokeReportPayloadBase(Data.JokeReport jokeReport)
        {
            JokeReport = jokeReport;
        }

        protected JokeReportPayloadBase(IReadOnlyList<UserError> errors): base(errors)
        {
        }

        public Data.JokeReport? JokeReport { get; }
    }
}