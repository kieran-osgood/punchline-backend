using System;
using System.Collections.Generic;
using GraphQL.Entities.UserJokeHistory;
using HotChocolate;
using HotChocolate.Types.Relay;

namespace GraphQL.Data
{
    public enum RatingValue
    {
        Bad = -1,
        Skip = 0,
        Good = 1,
        Reported = 2
    }

    public class UserJokeHistory
    {
        /*
         * Fields
         */
        public int Id { get; set; }
        public bool Bookmarked { get; set; }
        public RatingValue Rating { get; set; }
        public DateTime CreatedAt { get; set; } = default;

        /**
         * Entity Mappings
         */
        [ID(nameof(User))]
        [GraphQLIgnore]
        public int UserId { get; set; }

        public User User { get; set; } = default!;

        [ID(nameof(Joke))] [GraphQLIgnore] public int JokeId { get; set; }
        public Joke Joke { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}