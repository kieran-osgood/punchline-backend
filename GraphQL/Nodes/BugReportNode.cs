using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Nodes;

[Node]
[ExtendObjectType(typeof(BugReport))]
public class BugReportNode
{
    [NodeResolver]
    public static Task<BugReport> GetBugReportAsync(
        int id,
        BugReportByIdDataLoader bugReportById,
        CancellationToken cancellationToken)
        => bugReportById.LoadAsync(id, cancellationToken);

}