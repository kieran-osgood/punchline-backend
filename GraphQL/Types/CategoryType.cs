using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace GraphQL.Types
{
    public class CategoryType: ObjectType<Category>
    {
        protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<CategoryByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

    }
    }
}