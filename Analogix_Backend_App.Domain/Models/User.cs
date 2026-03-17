using Analogix_Backend_App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UserRoles Role { get; set; }
        public PlayerProfile? PlayerProfile { get; set; } 


        private User() { } // Private constructor for EF Core

        public User(string username, string password, string email, UserRoles role)
        {

            if (username == null && (username.Length < 3 || username.Length > 50)) 
            {
                throw new ArgumentException("Username cannot be null and must be between 3 and 20 characters."); // This checks if the username is null or if its length is less than 3 or greater than 50 characters.
            }

            if (string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email,out _)) 
            { 
                throw new ArgumentException("Invalid email format.", nameof(email)); // This checks if the email is null, empty, or whitespace, and also validates the email format using MailAddress.TryCreate.
            }

            Username = username;
            Password = password;
            Email = email;
            Role = UserRoles.User; // Default role is set to User!


        }


        public User (long id, string username, string password, string email, UserRoles role) 
            :this (username, password, email, role)
        {
            Id = id;
            

        }



    }

    

}
