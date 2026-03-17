using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class PlayerProfileRepository : IPlayerProfileRepository
    {
        public readonly AppDbContext _dbContext; 

        public PlayerProfileRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public PlayerProfile CreatePlayerProfile(PlayerProfile data) 
        {
            EntityEntry<PlayerProfile> entry = _dbContext.PlayerProfiles.Add(data);
            _dbContext.SaveChanges();
            var result = entry.Entity;
            return new PlayerProfile (result.Biography, result.FavoriteGames, result.MasteryLevel, result.UserId);
        }

        public PlayerProfile? GetPlayerProfileByUserId(long userId)
        {
            return _dbContext.PlayerProfiles.SingleOrDefault(p => p.UserId == userId);
        }

        public PlayerProfile UpdatePlayerProfile(PlayerProfile data)
        {
            var existing = _dbContext.PlayerProfiles.SingleOrDefault(p => p.UserId == data.UserId);
            if (existing is null) { throw new InvalidOperationException("No player profile exists for this user."); }

            existing.Biography = data.Biography;
            existing.FavoriteGames = data.FavoriteGames;
            existing.MasteryLevel = data.MasteryLevel;  
            
            _dbContext.SaveChanges(); 
            return existing;
        }
    }

    
}
