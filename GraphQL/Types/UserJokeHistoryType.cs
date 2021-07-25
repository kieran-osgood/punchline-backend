using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace GraphQL.Types
{
    public class UserJokeHistoryType : ObjectType<UserJokeHistory>
    {
        protected override void Configure(IObjectTypeDescriptor<UserJokeHistory> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<UserJokeHistoryByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.Joke)
                .ResolveWith<FavoritePracticeResolvers>(t => t.GetJokeAsync(default!, default!, default!))
                .Name("joke");
            
            descriptor
                .Field(t => t.User)
                .ResolveWith<FavoritePracticeResolvers>(t => t.GetUserAsync(default!, default!, default!))
                .Name("user");
        }

        private class FavoritePracticeResolvers
        {
            public async Task<Joke> GetJokeAsync(
                UserJokeHistory userJokeHistory,
                JokeByIdDataLoader dataLoader,
                CancellationToken cancellationToken) =>
                await dataLoader.LoadAsync(userJokeHistory.JokeId, cancellationToken);
            
            public async Task<User> GetUserAsync(
                UserJokeHistory userJokeHistory,
                UserByIdDataLoader dataLoader,
                CancellationToken cancellationToken) =>
                await dataLoader.LoadAsync(userJokeHistory.UserId, cancellationToken);
        }
    }
}