using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Nodes;

[Node]
[ExtendObjectType(typeof(BugReport))]
public class BugReportNode : ObjectType<BugReport>
{
}