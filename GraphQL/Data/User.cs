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
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public bool OnboardingComplete { get; set; } = false;

        /**
         * Entity Mappings
         */
        public List<UserJokeHistory> UserJokeHistories { get; set; } = new();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Joke> Jokes { get; set; } = new List<Joke>();
        public virtual ICollection<JokeReport> JokeReports { get; set; } = new List<JokeReport>();

        /*
         * Unmapped Resolver Fields
         */
    }
}