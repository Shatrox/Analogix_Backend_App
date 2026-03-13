using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Models;
using Analogix_Backend_App.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext; // This line declares a private readonly field of type AppDbContext, which will be used to interact with the database.
        
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User CreateUser(User data)
        {
            // adding user to database
            EntityEntry<User> element = _dbContext.Users.Add(data);
            // saving changes to database
            _dbContext.SaveChanges();
            // returning the created user
            var result = element.Entity; 
            return new User(result.Id, result.Username, result.Password, result.Email, result.Role); // returning a new User instance with the created user's details
        }

        public string GetPasswordByEmail(string email)
        {
            return _dbContext.Users
                .Single(m => m.Email == email) // querying the Users DbSet to find a single user with the specified email
                .Password; // returning the password of the found user

        }

        public User GetUserByEmail(string email)
        {
            var result = _dbContext.Users
                .Single(m => m.Email == email); // querying the Users DbSet to find a single user with the specified email
            return new User(result.Id, result.Username, null, result.Email, result.Role); // returning a new User instance with the found user's details, but with the password set to null for security reasons
        }
    }
}
