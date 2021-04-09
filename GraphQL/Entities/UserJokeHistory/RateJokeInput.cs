using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Entities.UserJokeHistory
{
    public record RateJokeInput([ID(nameof(Joke))] int JokeId, RatingValue Rating, bool Bookmarked = false);
}