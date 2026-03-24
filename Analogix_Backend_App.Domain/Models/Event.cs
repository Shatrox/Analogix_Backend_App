using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class Event
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxParticipants { get; set; }
        

        public long CreatorId { get; set; }
        public User Creator { get; set; } = default!;
        public ICollection<EventSubscription> Subscriptions { get; set; } = new List<EventSubscription>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>(); 
        public ICollection<EventFaq> EventFaq { get; set; } = new List<EventFaq>();
        public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();

        private Event() { } // Private constructor for EF Core  


        public Event(string title, string? description, string location, DateTime startDate,  DateTime? endDate,  int maxParticipants, long creatorId) 
        { 
        
            if(string.IsNullOrWhiteSpace(title) || title.Length > 120) 
            {

                throw new ArgumentException("Title is required and has a limit of 120 caracteres.", nameof(title));            
            
            
            }

            if(string.IsNullOrWhiteSpace(location) || location.Length > 120) 
            {
                throw new ArgumentException("Location is required and has a limit of 120 caracteres.", nameof(location));


            }

            if(startDate < DateTime.Now) 
            {
                throw new ArgumentException("Start date must be in the future.", nameof(startDate));
            }

            if(endDate < startDate)
            { 
            
                throw new ArgumentException("End date must be after the start date.", nameof(endDate));

            }

            if(maxParticipants < 2) 
            { 
            
                throw new ArgumentException("Max participants must be at least 2.", nameof(maxParticipants));

            }

            Title = title;
            Description = description;
            Location = location;
            StartDate = startDate;
            EndDate = endDate;
            MaxParticipants = maxParticipants;
            CreatorId = creatorId;

        }

        public Event(long id, string title, string? description, string location, DateTime startDate, DateTime endDate, int maxParticipants, long creatorId) 
                : this(title, description, location, startDate, endDate, maxParticipants, creatorId)
        {
                Id = id;
        }




    }
}
