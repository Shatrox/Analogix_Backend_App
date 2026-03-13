using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IUserService
    {
        User Login(string email, string password);
        User Register(User user);


    }
}
