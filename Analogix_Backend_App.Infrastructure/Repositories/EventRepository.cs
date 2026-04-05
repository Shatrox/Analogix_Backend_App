using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _dbContext;

        public EventRepository(AppDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }


        public Event Create(Event data, IReadOnlyCollection<string>? gameTags) // allow to create an event
        {
            var tags = ResolveTags(gameTags);

            foreach (var tag in tags)
            {
                data.GameTags.Add(tag);
            }
            var result = _dbContext.Events.Add(data);

            _dbContext.SaveChanges();

            return result.Entity;
        }

        public Event Update(Event data, IReadOnlyCollection<string>? gameTags)
        {
            var existingEvent = _dbContext.Events
                .Include(e => e.GameTags) // include related GameTags for updating
                .SingleOrDefault(e => e.Id == data.Id); // find the existing event by ID

            if (existingEvent == null) 
            {
                throw new InvalidOperationException("Event not found");
            }

            existingEvent.Title = data.Title;
            existingEvent.Description = data.Description;
            existingEvent.Location = data.Location;
            existingEvent.StartDate = data.StartDate;
            existingEvent.EndDate = data.EndDate;

            SyncTags(existingEvent.GameTags,gameTags);

            _dbContext.SaveChanges();

            return existingEvent;
        }

        public void Delete(Event data)
        {
            _dbContext.Events.Remove(data);
            _dbContext.SaveChanges();

        }

        public List<Event> GetAll(string? gameTag = null)
        {
            // allow to retrieve all events, optionally filtering by a specific game tag
            IQueryable<Event> query = _dbContext.Events
                .Include(e => e.GameTags)
                .Include(e => e.Creator)
                .Where(e => e.StartDate > DateTime.UtcNow);
                

            // checks if a game tag is provided
            if (!string.IsNullOrWhiteSpace(gameTag)) 
            { 
                // if yes, it normalizes the game tag
                string normalized = GameTag.Normalize(gameTag);
                // and filters the events to include only those that have a matching normalized game tag
                query = query.Where(e => e.GameTags.Any(t => t.NormalizedName == normalized));


            }


            // otherwise, it retrieves all events without filtering and orders them by their start date before returning the list
            return query
                .OrderBy(e => e.StartDate)
                .ToList();
        }

        public List<Event> GetEventsNotOwnedByUser(long userId)
        {
            // allow to retrieve events that are not owned by a specific user
            return _dbContext.Events
                .Include(e => e.GameTags)
                .Include(e => e.Creator)
                .Where(e => e.CreatorId != userId && e.StartDate > DateTime.UtcNow)
                .OrderBy(e => e.StartDate)
                .ToList();
        }

        public Event? GetById(long id) 
        {
           return _dbContext.Events
                .Include(e => e.GameTags)
                .Include(e => e.Creator)
                .Include(e => e.Subscriptions.Where(s => s.Status == SubscriptionStatus.Accepted))
                    .ThenInclude(s => s.User)
                .SingleOrDefault(e => e.Id == id);

        }

        public Event? GetByIdWithSubscriptions(long id) 
        {
            return _dbContext.Events
                .Include(e => e.Subscriptions)
                .Include(e => e.GameTags)
                .SingleOrDefault(e => e.Id == id);

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

        public Event TransferOwnership(long eventId, long newOwnerId)
        {
            Event existingEvent = _dbContext.Events.SingleOrDefault(e => e.Id == eventId) ?? throw new KeyNotFoundException("Event Not Found!");

            existingEvent.CreatorId = newOwnerId;

            _dbContext.SaveChanges();

            return existingEvent;
        }

        public List<Event> GetEventsUserParticipated(long userId)
        {
            return _dbContext.Events
                .Include(e => e.GameTags)
                .Include(e => e.Creator)
                .Include(e => e.Subscriptions)
                    .ThenInclude(s => s.User) 
                .Where(e => e.CreatorId == userId|| e.Subscriptions.Any(es => es.UserId == userId))
                .ToList();
        }

        public List<Event> GetMyEvents(long userId)
        {
            return _dbContext.Events
                .Include(e => e.GameTags)
                .Include(e => e.Creator)
                .Include(e => e.Subscriptions.Where(s => s.Status == SubscriptionStatus.Accepted))
                    .ThenInclude(s => s.User)
                .Where(e => e.CreatorId == userId)
                .ToList();
        }


    }
}
