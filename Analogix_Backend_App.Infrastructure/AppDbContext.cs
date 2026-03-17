using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Analogix_Backend_App.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } // This line defines a DbSet property for the User entity, allowing you to perform CRUD operations on the Users table in the database.
        public DbSet<PlayerProfile> PlayerProfiles { get; set; } // This line defines a DbSet property for the PlayerProfile entity, allowing you to perform CRUD operations on the PlayerProfiles table in the database.


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // Ctor Definition: allows for dependency injection.

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // This line applies all entity configurations defined in the current assembly. It looks for classes that implement IEntityTypeConfiguration<T> and applies their configurations to the model builder.

        }
    }
}
