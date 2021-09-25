using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FirebaseAdmin.Auth;

namespace GraphQL.Data
{
    public class JokeReport 
    {
        /*
         * Primary Key
         */
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