using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // Method to create a new user
        User CreateUser(User data);
        // Method to get a user by their email
        User GetUserByEmail(string email);
        // Allow to get password by email
        string  GetPasswordByEmail(string email);






    }
}
