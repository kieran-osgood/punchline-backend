using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data
{
    public class Joke
    {
        /*
         * Fields
         */
        public int Id { get; set; }

        [Required] [StringLength(200)] public string Body { get; set; }

        /**
         * Entity Mappings
         */
        public virtual ICollection<Category> Categories { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}