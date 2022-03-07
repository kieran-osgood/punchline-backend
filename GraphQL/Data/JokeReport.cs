using System;
using HotChocolate.Types.Relay;

namespace GraphQL.Data
{
    public class JokeReport 
    {
        /*
         * Primary Key
         */
        [ID]
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime CreatedAt { get; set; } = default;

        /**
         * Entity Mappings
         */
        public Joke ReportedJoke { get; set; } = new();
        public User ReportingUser { get; set; } = new();

        /*
         * Unmapped Resolver Fields
         */
    }
}