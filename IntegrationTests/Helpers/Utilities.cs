using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GraphQL.Data;

namespace IntegrationTests.Helpers
{
    public static class Utilities
    {
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
                    Body = "What do you call a cow with no legs? Ground Beef!",
                    Id = 1,
                    Title = "Cow With No Legs",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body = "What do you call a cow jumping over a barbed wire fence? Utter destruction.",
                    Id = 2,
                    Title = "Jumping Cow",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body = "What's black and white and red all over? A newspaper.",
                    Id = 4,
                    Title = "Black, White and Red",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body = "So, this guy walks into a bar. And says, \"ouch\".",
                    Id = 5,
                    Title = "Guy in a Bar",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body = "If the opposite of pro is con, isn't the opposite of progress, congress?",
                    Id = 6,
                    Title = "Progress",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body = "What do you call a guy with no arms or legs floating in the ocean? Bob!",
                    Id = 7,
                    Title = "Guy with no Limbs",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body =
                        "I went to a wedding the other day.  Two antennas were getting married.  It wasn't much of a wedding ceremony, but it was one heck of a reception!",
                    Id = 8,
                    Title = "Antenna",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body = "There's this dyslexic guy... he walked into a bra...",
                    Id = 9,
                    Title = "Into the Bar",
                    Length = JokeLength.Small
                },
                new()
                {
                    Body =
                        "Joel: \"How's the progress on new house that you are building Pete?\" Peter: \"Things are really slow at the moment.\" Joel: \"Yeah, I guess all this rain would be putting a dampener on things...\"",
                    Id = 10,
                    Title = "Rain",
                    Length = JokeLength.Medium
                }
            };
        }

        private static IEnumerable<Category> GetSeedingCategories()
        {
            return new List<Category>
            {
                new()
                {
                    Id = 1,
                    Name = "TEST RECORD: You're standing on my scarf."
                },
                new()
                {
                    Id = 2,
                    Name = "TEST RECORD: Would you like a jelly baby?"
                },
                new()
                {
                    Id = 3,
                    Name = "TEST RECORD: To the rational mind, " +
                           "nothing is inexplicable; only unexplained."
                }
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
                    LastLogin = new DateTime(),
                    Categories = new List<Category>
                    {
                        new()
                        {
                            Id = 4,
                            Name = "TEST RECORD: this is a user category",
                            Image = "s3:url"
                        }
                    }
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

        public static Joke GetValidJoke()
        {
            return GetSeedingJokes().FirstOrDefault();
        }
    }
}