using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly IPlayerProfileRepository _playerProfileRepository;

        public PlayerProfileService(IPlayerProfileRepository playerProfileRepository)
        {
            _playerProfileRepository = playerProfileRepository;
        }


        public PlayerProfile Create(long userID, string? biography, string favoriteGames, MasteryLevel masteryLevel, IReadOnlyCollection<string>? favoriteGameTags) 
        {
            if (_playerProfileRepository.GetPlayerProfileByUserId(userID) is not null) 
            { 
                
               throw new InvalidOperationException("A player profile already exists for this user.");

            }

            PlayerProfile profile = new PlayerProfile 
            ( 
                biography, 
                favoriteGames, 
                masteryLevel, 
                userID
            );


            return _playerProfileRepository.CreatePlayerProfile(profile, favoriteGameTags);
        }

        public PlayerProfile Update(long userID, string? biography, string favoriteGames, MasteryLevel masteryLevel, IReadOnlyCollection<string>? favoriteGameTags) 

        { 
        
            if(_playerProfileRepository.GetPlayerProfileByUserId(userID) is null)
            {
                throw new InvalidOperationException("No player profile exists for this user.");
            }

            PlayerProfile profileToUpdate = new PlayerProfile
            (
                biography,
                favoriteGames,
                masteryLevel,
                userID
            );

            return _playerProfileRepository.UpdatePlayerProfile(profileToUpdate, favoriteGameTags);
        }

        public PlayerProfile? GetByUserId(long userID)
        {
            return _playerProfileRepository.GetPlayerProfileByUserId(userID);
        }


    }
}
