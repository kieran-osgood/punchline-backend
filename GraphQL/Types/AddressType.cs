using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace GraphQL.Types
{
    public class AddressType: ObjectType<Joke>
    {
        protected override void Configure(IObjectTypeDescriptor<Joke> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<JokeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

    }
    }
}