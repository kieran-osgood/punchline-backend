using System;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Joke> Jokes { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<UserJokeHistory> UserJokeHistory { get; set; } = default!;
        public DbSet<JokeReport> JokeReports { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Joke>()
                .Property(e => e.Length)
                .HasConversion<string>();
            
            modelBuilder
                .Entity<Joke>(entity =>
                {
                    entity
                        .HasMany(t => t.Categories)
                        .WithMany(t => t.Jokes);

                    entity
                        .HasMany(x => x.UserJokeHistories)
                        .WithOne(x => x.Joke);
                });

            modelBuilder
                .Entity<Category>(entity =>
                {
                    entity
                        .HasMany(t => t.Jokes)
                        .WithMany(t => t.Categories);
                });

            modelBuilder
                .Entity<User>(entity =>
                {
                    entity
                        .HasMany(t => t.Jokes)
                        .WithMany(t => t.Users);

                    entity
                        .HasMany(t => t.Categories)
                        .WithMany(t => t.Users);
                });

            modelBuilder.Entity<User>()
                .HasMany(p => p.Jokes)
                .WithMany(p => p.Users)
                .UsingEntity<UserJokeHistory>(
                    x => x
                        .HasOne(ujh => ujh.Joke)
                        .WithMany(j => j.UserJokeHistories)
                        .HasForeignKey(ujh => ujh.JokeId),
                    x => x
                        .HasOne(ujh => ujh.User)
                        .WithMany(u => u.UserJokeHistories)
                        .HasForeignKey(ujh => ujh.UserId),
                    x => { x.HasIndex(ujh => new {ujh.JokeId, ujh.UserId}).IsUnique(); });
            
            modelBuilder
                .Entity<JokeReport>(entity =>
                {
                    entity
                        .HasOne(t => t.ReportingUser)
                        .WithMany(t => t.JokeReports);
                    entity
                        .HasOne(t => t.ReportedJoke)
                        .WithMany(t => t.JokeReports);
                });
        }
    }
}