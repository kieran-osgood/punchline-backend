using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Entities.User
{
    public record UserLoginInput(string FirebaseUid, string Username);
}