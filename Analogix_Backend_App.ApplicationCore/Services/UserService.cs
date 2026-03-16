using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using Soenneker.Hashing.Argon2;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Analogix_Backend_App.Domain.BusineesExceptions.UserException;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class UserService : IUserService // This class implements the IUserService interface, providing methods for user authentication and registration.
    {

        private readonly IUserRepository _userRepository; // This line declares a private readonly field of type IUserRepository, which will be used to interact with the user data repository.

        public UserService(IUserRepository userRepository) 
        {

            _userRepository = userRepository;
        
        
        
        }

        public User Login(string email, string password) // This line defines the Login method, which takes an email and password as parameters and returns a User object.
        {
           string hashpwd = _userRepository.GetPasswordByEmail(email);

            if (hashpwd is null)
            {
                throw new UserBadCredentialException(); // Invalid email, user not found
            }

            bool isPasswordValid = Argon2HashingUtil.Verify(password, hashpwd).Result; // This line verifies the provided password against the stored hashed password using the Argon2 hashing algorithm.
            if (!isPasswordValid)
            {
                throw new UserBadCredentialException(); // Invalid password
            }

            return _userRepository.GetUserByEmail(email); // If the email and password are valid, this line retrieves and returns the User object associated with the provided email from the user repository.
        }

        public User Register(User user) // This line defines the Register method, which takes a User object as a parameter and returns a User object after registration.
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Username, password, and email cannot be empty.");
            }

            string hashpwd = Argon2HashingUtil.Hash(user.Password).Result; // This line hashes the user's password using the Argon2 hashing algorithm.

            // Create a new User object with the hashed password and other user details.
            var role = UserRoles.User; // Default role is set to User, you can modify this as needed.
            User newUserToAdd = new User(

                user.Username,
                hashpwd,
                user.Email,
                role
            );

            return _userRepository.CreateUser(newUserToAdd); // This line calls the CreateUser method of the IUserRepository to save the new user to the database and returns the created User object.

            
        }
    }
}
