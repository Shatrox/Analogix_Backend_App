using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Models;
using Soenneker.Hashing.Argon2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class UserService : IUserService // This class implements the IUserService interface, providing methods for user authentication and registration.
    {

        private readonly IUserRepository _userRepository; // This line declares a private readonly field of type IUserRepository, which will be used to interact with the user data repository.



        public User Login(string email, string password) // This line defines the Login method, which takes an email and password as parameters and returns a User object.
        {
            throw new NotImplementedException();
        }

        public User Register(User user) // This line defines the Register method, which takes a User object as a parameter and returns a User object after registration.
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Username, password, and email cannot be empty.");
            }

            string hashpwd = Argon2HashingUtil.Hash(user.Password).Result; // This line hashes the user's password using the Argon2 hashing algorithm.

            // Create a new User object with the hashed password and other user details.



        }
    }
}
