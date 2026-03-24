using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public PlayerProfile CreatePlayerProfile(PlayerProfile data, IReadOnlyCollection<string>? favoriteGameTags) 
        {
            var tags = ResolveTags(favoriteGameTags);

            foreach (var tag in tags)
            {
                data.FavoriteGameTags.Add(tag);
            }

            EntityEntry<PlayerProfile> entry = _dbContext.PlayerProfiles.Add(data);
            _dbContext.SaveChanges();
            var result = entry.Entity;
            return new PlayerProfile (result.Biography, result.FavoriteGames, result.MasteryLevel, result.UserId);
        }

        public PlayerProfile? GetPlayerProfileByUserId(long userId)
        {
            return _dbContext.PlayerProfiles.SingleOrDefault(p => p.UserId == userId);
        }

        public PlayerProfile UpdatePlayerProfile(PlayerProfile data, IReadOnlyCollection<string>? favoriteGameTags)
        {
            var existing = _dbContext.PlayerProfiles
                .Include(p => p.FavoriteGameTags)
                .SingleOrDefault(p => p.UserId == data.UserId);
            if (existing is null) { throw new InvalidOperationException("No player profile exists for this user."); }

            existing.Biography = data.Biography;
            existing.FavoriteGames = data.FavoriteGames;
            existing.MasteryLevel = data.MasteryLevel;

            SyncTags(existing.FavoriteGameTags, favoriteGameTags);
            
            _dbContext.SaveChanges(); 
            return existing;
        }


        private void SyncTags(ICollection<GameTag> currentTag, IReadOnlyCollection<string>? requestedTag)
        {
            // allow to update the tags of an event, by comparing the current tags with the requested tags and adding or removing tags as necessary
            currentTag.Clear();
            var tags = ResolveTags(requestedTag);
            foreach (var tag in tags)
            {
                currentTag.Add(tag);
            }




        }

        private List<GameTag> ResolveTags(IReadOnlyCollection<string>? tagNames)
        {
            // Removes empty values, extra spaces, and duplicates from the input collection
            var clean = (tagNames ?? Array.Empty<string>())
                .Select(t => t.Trim()) // remove extra spaces
                .Where(t => !string.IsNullOrEmpty(t)) // empty values
                .Distinct() // prevent duplicates
                .ToList();

            // If there are no valid tags after cleaning, return an empty list
            if (clean.Count == 0)
            {
                return new List<GameTag>();
            }
            // Normalize the cleaned tag names to ensure consistent formatting
            var normalized = clean.Select(GameTag.Normalize).ToList();

            // Query the database for existing GameTag entities that match the normalized tag names
            var existingTags = _dbContext.GameTags
                .Where(t => normalized.Contains(t.NormalizedName))
                .ToList();
            //
            var byNormalized = existingTags.ToDictionary(t => t.NormalizedName, t => t);
            var result = new List<GameTag>();

            foreach (var tagName in clean)
            {
                var normalizedTagName = GameTag.Normalize(tagName);
                if (!byNormalized.TryGetValue(normalizedTagName, out var tag))
                {
                    tag = new GameTag(tagName);
                    _dbContext.GameTags.Add(tag);
                    byNormalized[normalizedTagName] = tag;
                }

                result.Add(tag);

            }

            return result;
        }



    }

    
}
