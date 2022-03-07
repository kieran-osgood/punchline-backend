using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace GraphQL.Nodes
{
    [Node]
    [ExtendObjectType(typeof(UserJokeHistory))]
    public class UserJokeHistoryNode
    {
        [NodeResolver]
        [BindMember(nameof(UserJokeHistory.Joke), Replace = true)]
        public async Task<Joke> GetJokeAsync(
            [Parent] UserJokeHistory userJokeHistory,
            JokeByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(userJokeHistory.JokeId, cancellationToken);
        
        [NodeResolver]
        [BindMember(nameof(UserJokeHistory.User), Replace = true)]
        public async Task<User> GetUserAsync(
            [Parent] UserJokeHistory userJokeHistory,
            UserByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(userJokeHistory.UserId, cancellationToken);
    }
}