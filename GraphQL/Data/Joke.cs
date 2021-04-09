using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data
{
    public class Joke
    {
        /*
         * Primary Key
         */
        public int Id { get; set; }

        /*
         * Fields
         */
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public int Score { get; set; }

        /**
         * Entity Mappings
         */
        public List<UserJokeHistory> UserJokeHistories { get; set; } = new();

        public virtual ICollection<Category> Categories { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }

    public enum JokeLength
    {
        Small = 175,
        Medium = 400,
        Large = 5000
    }
}