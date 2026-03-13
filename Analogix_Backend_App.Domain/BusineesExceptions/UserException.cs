using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.BusineesExceptions
{
    public class UserException : Exception
    {
        // Constructor that takes a message and passes it to the base Exception class
        public UserException(string message) : base(message)
        {
        }

        // Exception for invalid credentials during login
        public class UserBadCredentialException : UserException
        {
            public UserBadCredentialException() : base("Invalid email or password.")
            {
            }
        }
        // Exception for trying to register with an email that already exists
        public class UserAlreadyExistsException : UserException
        {
            public UserAlreadyExistsException() : base("A user with this email already exists.")
            {
            }
        }

    }
}
