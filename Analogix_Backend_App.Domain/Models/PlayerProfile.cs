using Analogix_Backend_App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class PlayerProfile
    {
        public long Id { get; set; }
        public string? Biography { get; set; } = default!;
        public string FavoriteGames { get; set; } = default!;
        public MasteryLevel MasteryLevel { get; set; } = default!;
        // Navigation property to User
        public User User { get; set; } = default!;
        // Foreign key to User
        public long UserId { get; set; }


        public ICollection<GameTag> FavoriteGameTags { get; set; } = new List<GameTag>(); 


        private PlayerProfile() { } // Private constructor for EF Core


        public PlayerProfile(string? biography, string favoriteGames, MasteryLevel masteryLevel, long userId)
        {
            Biography = biography;
            FavoriteGames = favoriteGames;
            MasteryLevel = masteryLevel;
            UserId = userId;
        }

        public PlayerProfile(long id, string? biography, string favoriteGames, MasteryLevel masteryLevel, long userId)
             : this(biography, favoriteGames, masteryLevel, userId)
        {
             Id = id;
        }
    }
}
