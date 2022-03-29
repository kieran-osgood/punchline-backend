using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Nodes
{
    [Node]
    [ExtendObjectType(typeof(UserJokeHistory))]
    public class UserJokeHistoryNode
    {
        [NodeResolver]
        public static Task<UserJokeHistory> GetUserJokeHistoryAsync(
            int id,
            UserJokeHistoryByIdDataLoader userJokeHistoryById,
            CancellationToken cancellationToken)
            => userJokeHistoryById.LoadAsync(id, cancellationToken);
        
        [BindMember(nameof(UserJokeHistory.Joke), Replace = true)]
        public async Task<Joke> GetJokeAsync(
            [Parent] UserJokeHistory userJokeHistory,
            JokeByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(userJokeHistory.JokeId, cancellationToken);
        
        [BindMember(nameof(UserJokeHistory.User), Replace = true)]
        public async Task<User> GetUserAsync(
            [Parent] UserJokeHistory userJokeHistory,
            UserByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(userJokeHistory.UserId, cancellationToken);
    }
}