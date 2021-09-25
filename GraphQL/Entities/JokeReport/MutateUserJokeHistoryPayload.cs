using System.Collections.Generic;
using GraphQL.Common;

namespace GraphQL.Entities.JokeReport
{
    public class MutateJokeReportPayload : JokeReportPayloadBase
    {
        public MutateJokeReportPayload(Data.JokeReport jokeReport) : base(jokeReport)
        {
        }

        public MutateJokeReportPayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}