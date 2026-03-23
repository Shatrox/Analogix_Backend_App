using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Analogix_Backend_App.Domain.Models
{
    public class EventFaq
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public Event Event { get; set; } = default!; // Navigation property to the related Event
        public long AuthorUserId { get; set; }
        public User AuthorUser { get; set; } = default!; // Navigation property to the related User (author of the FAQ)
        public string Question { get; set; } = default!;
        public DateTime AskedAtUtc { get; set; } = default!;
        public string? Answer { get; set; }
        public long? AnsweredUserId { get; set; }
        public User? AnsweredUser { get; set; } = default!; // Navigation property to the related User (who answered the FAQ)
        public DateTime? AnsweredAtUtc { get; set; }
        

        private EventFaq() { } // Private constructor for EF Core

        public EventFaq(long eventId, long authorUserId, string question)
        {
            // Validate input parameters
            if (eventId <= 0)
            {
                throw new ArgumentException("Event ID must be a positive number.", nameof(eventId));
            }
            if (authorUserId <= 0)
            {
                throw new ArgumentException("Author User ID must be a positive number.", nameof(authorUserId));
            }
            if (string.IsNullOrWhiteSpace(question))
            {
                throw new ArgumentException("Question cannot be null or empty.", nameof(question));
            }

            // Limitate the length of the question to 1000 characters
            if (question.Length > 1000)
            {
                throw new ArgumentException("Question cannot exceed 1000 characters.", nameof(question));
            }

            EventId = eventId;
            AuthorUserId = authorUserId;
            Question = question;
            AskedAtUtc = DateTime.UtcNow;
        }

        public void AddAnswer(long answeredUserId, string answer)
        {
            // checks inputs
            if (answeredUserId <= 0) { throw new ArgumentOutOfRangeException(nameof(answeredUserId)); }

            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer is required", nameof(answer));
            }

            if (answer.Length > 1000)
            {
                throw new ArgumentException("Answer cannot exceed 1000 characteres");
            }

           // if (answer is not null)
            //{
                //throw new InvalidOperationException("Question already replyed.");
            //}

            Answer = answer?.Trim();
            AnsweredUserId = answeredUserId;
            AnsweredAtUtc = DateTime.UtcNow;

        }
        
        public void RemoveAnswer() 
        { 
            Answer = null;
            AnsweredUserId = null;
            AnsweredAtUtc = null;
        }
    }
}
