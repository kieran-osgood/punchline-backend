using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQL.Data
{
    public class Category
    {
        /*
         * Primary Key
         */
        public int Id { get; set; }

        /*
         * Fields
         */
        public string Name { get; set; } = "";
        public string? Image { get; set; } = "";

        /**
         * Entity Mappings
         */
        public virtual ICollection<Joke> Jokes { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
        [NotMapped]
        public virtual Joke Joke { get; set; } = default!;
    }
}