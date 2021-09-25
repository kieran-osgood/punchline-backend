using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<UserByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.Categories)
                .ResolveWith<UserResolvers>(t => t.GetUserCategoriesAsync(default!, default!, default!, default!))
                .UseDbContext<ApplicationDbContext>()
                .Name("categories");
        }

        private class UserResolvers
        {
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

                return await dataLoader.LoadAsync(ids, cancellationToken);
            }
        }
    }
}