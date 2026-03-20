using Analogix_Backend_App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class Rating
    {
        public long Id { get; set; }

        public long EventId { get; set; }
        public Event Event { get; set; } = default!;

        public long RaterUserId { get; set; }
        public User RaterUser { get; set; } = default!;

        public long? TargetUserId { get; set; }
        public User? TargetUser { get; set; } = default!;

        public RatingTargetType TargetType { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; }


        private Rating() { } // Private constructor for EF Core

        public Rating(long eventId, long raterUserId, RatingTargetType targetType, int score, long? targetUserId = null)
        {
            // Check Id's validity
            if (eventId <= 0)
            {
                throw new ArgumentException("Event ID must be a positive number.", nameof(eventId));
            }

            if (raterUserId <= 0)
            {
                throw new ArgumentException("Rater User ID must be a positive number.", nameof(raterUserId));
            }

            // Score cannot be less than 1 or greater than 5
            if (score < 1 || score > 5) 
            {
                throw new ArgumentException("Score must be between 1 and 5.", nameof(score));
            }

            // If the target type is Player, a valid Target User ID must be provided
            if (targetType == RatingTargetType.Player && (!targetUserId.HasValue || targetUserId.Value <= 0)) 
            {
                throw new ArgumentException("Target User ID must be provided and positive when TargetType is Player.", nameof(targetUserId));
            }
            
            if (targetType == RatingTargetType.Event && targetUserId.HasValue) 
            {
                throw new ArgumentException("Target User ID should not be provided when TargetType is Event.", nameof(targetUserId));
            }
            
            // Prevent users from rating themselves
            if (targetUserId == raterUserId) 
            {
                throw new ArgumentException("You can't rate yourself", nameof(targetUserId));
            }


            EventId = eventId;
            RaterUserId = raterUserId;
            TargetType = targetType;
            TargetUserId = targetUserId;
            Score = score;
            CreatedAt = DateTime.UtcNow;


            
        }

    }
}
