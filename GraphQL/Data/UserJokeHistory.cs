using System.Collections.Generic;

namespace GraphQL.Data
{
    public class UserJokeHistory
    {
        /*
         * Fields
         */
        public bool Bookmarked { get; set; }
        public int Rating { get; set; }

        /**
         * Entity Mappings
         */
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public int JokeId { get; set; }
        public Joke Joke { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}