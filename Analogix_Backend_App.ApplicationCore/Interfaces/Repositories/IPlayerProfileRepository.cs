using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IPlayerProfileRepository
    {
        // Method to create a new player profile
        PlayerProfile CreatePlayerProfile(PlayerProfile data);

        // Method to get a player profile by user ID
        PlayerProfile GetPlayerProfileByUserId(long userId);

        // Method to update an existing player profile
        PlayerProfile UpdatePlayerProfile(PlayerProfile data);


    }
}
