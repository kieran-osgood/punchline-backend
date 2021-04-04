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
        }
    }
}