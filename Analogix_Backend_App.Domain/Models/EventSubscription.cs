using Analogix_Backend_App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class EventSubscription
    {
        public long Id { get; set; }

        public long EventId { get; set; }
        public Event Event { get; set; } = default!;

        public long UserId { get; set; }
        public User User { get; set; } = default!;

        public SubscriptionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResponseAt { get; set; }


        private EventSubscription() { } // Private constructor for EF Core

        public EventSubscription(long eventId, long userId)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("Event ID must be a positive number.", nameof(eventId));
            }
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive number.", nameof(userId));
            }

            EventId = eventId;
            UserId = userId;
            Status = SubscriptionStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            ResponseAt = null; // Response time will be set when the status changes from Pending to Accepted or Refused
        }


        public EventSubscription(long id, long eventId, long userId, SubscriptionStatus status, DateTime createdAt, DateTime? responseAt)
        {
            Id = id;
            EventId = eventId;
            UserId = userId;
            Status = status;
            CreatedAt = createdAt;
            ResponseAt = responseAt;
        }

        public void SetStatus(SubscriptionStatus newStatus)
        {
            if (newStatus != SubscriptionStatus.Pending && newStatus != SubscriptionStatus.Deleted && newStatus != SubscriptionStatus.Accepted && newStatus != SubscriptionStatus.Refused )
            {
                throw new ArgumentException("", nameof(newStatus));
            }
            Status = newStatus;
            ResponseAt = DateTime.UtcNow;
        }


    }
}
