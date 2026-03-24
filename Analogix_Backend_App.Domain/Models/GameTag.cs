using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class GameTag
    {
        public long Id { get; set; }
        public string Name { get; private set; } = default!;
        public string NormalizedName { get; private set; } = default!;

        public ICollection<PlayerProfile> PlayerProfiles { get; set; } = new List<PlayerProfile>(); // Navigation
        public ICollection<Event> Events { get; set; } = new List<Event>(); // Navigation


        private GameTag() { } // For EF Core


        public GameTag(string name)
        {

            SetName(name);

        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
                throw new ArgumentException("Game tag name cannot be null or empty, neither have more than 50 characters.", nameof(name));

            Name = name.Trim();
            NormalizedName = Normalize(name); 
        }

        public static string Normalize(string value) => value.Trim().ToUpperInvariant(); // Normalize for case-insensitive comparisons ex: "PoTaTo" -> "POTATO" and prevents whitespace issues
    }
}
