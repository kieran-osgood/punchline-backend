using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GraphQL.Data;

namespace IntegrationTests.Helpers
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Jokes.AddRange(GetSeedingJokes());
            db.Categories.AddRange(GetSeedingCategories());
            db.Users.AddRange(GetSeedingUsers());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Jokes.RemoveRange(db.Jokes);
            db.Categories.RemoveRange(db.Categories);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
            InitializeDbForTests(db);
        }

        private static IEnumerable<Joke> GetSeedingJokes()
        {
            return new List<Joke>
            {
                new()
                {
                    Categories = GetSeedingCategories().ToList(),
                    Body = "TEST RECORD: You're standing on my scarf."
                },
                new(){ Body = "TEST RECORD: Would you like a jelly baby?" },
                new(){ Body = "TEST RECORD: To the rational mind, " +
                              "nothing is inexplicable; only unexplained." }
            };
        }

        private static IEnumerable<Category> GetSeedingCategories()
        {
            return new List<Category>
            {
                new(){ Name = "TEST RECORD: You're standing on my scarf." },
                new(){ Name = "TEST RECORD: Would you like a jelly baby?" },
                new(){ Name = "TEST RECORD: To the rational mind, " +
                              "nothing is inexplicable; only unexplained." }
            };
        }
        
        private static IEnumerable<User> GetSeedingUsers()
        {
            return new List<User>
            {
                new()
                {
                    Id = 1,
                    FirebaseUid = "n3mU54T2ZJTrS7DySfDtPf6dB9M2",
                    Name = "Test User",
                    JokeCount = 0,
                    CreatedOn = new DateTime(),
                    LastLogin = new DateTime()
                    
                },
            };
        }

        /**
         * Returns a User, useful within tests when you you want an id that *should* return a user
         * from the HttpInterceptors
         */
        public static User GetValidAuthUser()
        {
            return GetSeedingUsers().FirstOrDefault();
        }
        #endregion
    }
}