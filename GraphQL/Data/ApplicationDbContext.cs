using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public DbSet<User> Users { get; set; } = default!;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public DbSet<Joke> Jokes { get; set; } = default!;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public DbSet<Category> Categories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Joke>(entity =>
                {
                    entity
                        .HasMany(t => t.Categories)
                        .WithMany(t => t.Jokes);
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
                    x =>
                    {
                        x.HasIndex(ujh => new {ujh.JokeId, ujh.UserId}).IsUnique();
                    });
        }
    }
}