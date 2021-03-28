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
        public virtual ICollection<Category> Categories { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}