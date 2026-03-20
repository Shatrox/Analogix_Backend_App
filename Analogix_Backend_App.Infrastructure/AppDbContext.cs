using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Analogix_Backend_App.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        // Here we define DbSet properties for each of our entities. These properties represent the tables in our database and allow us to perform CRUD operations
        public DbSet<User> Users { get; set; } 
        public DbSet<PlayerProfile> PlayerProfiles { get; set; } 
        public DbSet<Event> Events { get; set; } 
        public DbSet<EventSubscription> EventSubscriptions { get; set; } 
        public DbSet<Rating> Ratings { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // Ctor Definition: allows for dependency injection.

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // This line applies all entity configurations defined in the current assembly. It looks for classes that implement IEntityTypeConfiguration<T> and applies their configurations to the model builder.

        }
    }
}
