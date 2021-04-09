using System.Collections.Generic;
using GraphQL.Entities.UserJokeHistory;
using HotChocolate.Types.Relay;

namespace GraphQL.Data
{
    public enum RatingValue
    {
        Bad = -1,
        Skip = 0,
        Good = 1
    }

    public class UserJokeHistory
    {
        /*
         * Fields
         */
        public bool Bookmarked { get; set; }
        public RatingValue Rating { get; set; }

        /**
         * Entity Mappings
         */
        [ID(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; } = default!;
        [ID(nameof(Joke))] public int JokeId { get; set; }
        public Joke Joke { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}