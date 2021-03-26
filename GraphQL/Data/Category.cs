using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data
{
    public class Category
    {
        /*
         * Fields
         */
        public int Id { get; set; }

        [Required] [StringLength(30)] public string Name { get; set; }

        /**
         * Entity Mappings
         */
        public virtual ICollection<Joke> Jokes { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}