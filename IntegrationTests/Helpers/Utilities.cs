using System.Collections.Generic;
using GraphQL.Data;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace IntegrationTests.Helpers
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Jokes.AddRange(GetSeedingJokes());
            db.Categories.AddRange(GetSeedingCategories());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Jokes.RemoveRange(db.Jokes);
            InitializeDbForTests(db);
        }

        private static IEnumerable<Joke> GetSeedingJokes()
        {
            return new List<Joke>
            {
                new(){ Id = 1, Body = "TEST RECORD: You're standing on my scarf." },
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

        #endregion
    }
}