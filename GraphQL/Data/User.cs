using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FirebaseAdmin.Auth;

namespace GraphQL.Data
{
    public class User
    {
        /*
         * Primary Key
         */
        public int Id { get; set; }
        public string FirebaseUid { get; set; } = "";

        /*
         * Fields
         */
        public int JokeCount { get; set; }
        public string Name { get; set; } = "";

        /**
         * Entity Mappings
         */
        public List<UserJokeHistory> UserJokeHistories { get; set; } = new();
        public virtual ICollection<Category> Categories { get; set; } = default!;
        public virtual ICollection<Joke> Jokes { get; set; } = default!;

        /*
         * Unmapped Resolver Fields
         */
    }
}