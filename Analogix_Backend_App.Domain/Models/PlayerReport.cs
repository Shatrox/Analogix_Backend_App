using Analogix_Backend_App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class PlayerReport
    {
        public long Id { get; set; }

        public long ReporterId { get; set; }
        public User Reporter { get; set; } = default!; // Navigation

        public long EventId { get; set; }
        public Event Event { get; set; } = default!;

        public long ReportedPlayerId { get; set; }
        public User ReportedPlayer { get; set; } = default!; 

        public ReasonsToReport Reason { get; set; } 
        public string Description { get; set; } = default!;
        public ReportStatus ReportStatus { get; set; } 
        public DateTime CreatedAt { get; set; } 

        public long? ReviewerId { get; set; }
        public User? Reviewer { get; set; } // Navigation

        public DateTime? ReviewedAtUtc { get; set; }
        public string? ReviewNote { get; set; }


        private PlayerReport () { }

        public PlayerReport(long reporterId, long eventId, long reportedPlayerId, ReasonsToReport reasonsToReport, string description) 
        { 
        
            if (reporterId <= 0) 
            {
                throw new ArgumentException("Reporter Id doesn't exist!", nameof(reporterId));
            }

            if (eventId <= 0) 
            {
                throw new ArgumentException("Event not found!", nameof(eventId));
            }

            if (reportedPlayerId <= 0) 
            {
                throw new ArgumentException("Reported Player not found!", nameof(reportedPlayerId));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description is required!", nameof(description));
            }

            if (reporterId == reportedPlayerId) 
            { 
                throw new ArgumentException("Reporter cannot report themselves!", nameof(reporterId));
            }
            
            description = description.Trim();

            if (description.Length < 10 || description.Length > 2000)
            {
                throw new ArgumentException("Description must be between 10 and 2000 characters!", nameof(description));
            }
        
            ReporterId = reporterId;
            EventId = eventId;
            ReportedPlayerId = reportedPlayerId;
            Reason = reasonsToReport;
            Description = description;
            ReportStatus = ReportStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        
        }

        public void ReportReview(long reviewerId, ReportStatus newStatus, string? reviewNote) 
        { 
        
            if (reviewerId <= 0) 
            { 
                throw new ArgumentException("Reviewer not found!", nameof(reviewerId)); 
            }

            if (newStatus == ReportStatus.Pending)
            {
                throw new ArgumentException("Report status cannot be set to pending during review!", nameof(newStatus));
            }

            if (ReportStatus != ReportStatus.Pending && ReportStatus != ReportStatus.InReview)
            {
                throw new ArgumentException("Only Pending or InReview reports can be reviewed");
            }

            if (!string.IsNullOrWhiteSpace(reviewNote)) 
            { 
                reviewNote = reviewNote.Trim();

                if (reviewNote.Length < 10 || reviewNote.Length > 2000) 
                { 
                    throw new ArgumentException("Review note must be between 10 and 2000 characters!", nameof(reviewNote));

                }
            }

            ReportStatus = newStatus;
            ReviewerId = reviewerId;
            ReviewedAtUtc = DateTime.UtcNow;
            ReviewNote = string.IsNullOrWhiteSpace(reviewNote) ? null : reviewNote;
        }
    }
}
