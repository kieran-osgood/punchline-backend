using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int PositiveRating { get; set; } = 0;
        public int NegativeRating { get; set; } = 0;
        public int SkipRating { get; set; } = 0;
        public int ReportCount { get; set; } = 0;
        
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