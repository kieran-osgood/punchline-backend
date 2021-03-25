using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data
{
    public class Joke
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Body { get; set; }

    }
}