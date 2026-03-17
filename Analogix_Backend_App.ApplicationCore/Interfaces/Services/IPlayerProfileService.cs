using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    // Interface for player profile service
    public interface IPlayerProfileService
    {
        PlayerProfile Create (long userID, string? biography, string favoriteGames, MasteryLevel masteryLevel);
        PlayerProfile Update (long userID, string? biography, string favoriteGames, MasteryLevel masteryLevel);

    }
}
