using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Nodes
{
    [Node]
    [ExtendObjectType(typeof(User))]
    public class UserNode
    {
        [NodeResolver]
        [BindMember(nameof(User.Categories), Replace = true)]
        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(
            [Parent] User user,
            CategoryByIdDataLoader dataLoader,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var ids = await (
                    from u in dbContext.Users
                    where u.Id == user.Id
                    select u.Categories.Select(x => x.Id).ToList())
                .FirstOrDefaultAsync(cancellationToken);

            return await dataLoader.LoadAsync(ids ?? new List<int> { }, cancellationToken);
        }
    }
}