using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class EventSubscriptionRepository : IEventSubscriptionRepository
    {
        private readonly AppDbContext _dbContext;

        public EventSubscriptionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EventSubscription Create(EventSubscription data)
        {
            var result = _dbContext.EventSubscriptions.Add(data);

            _dbContext.SaveChanges();

            return result.Entity;
        }

        public EventSubscription Update(EventSubscription data)
        {
            var existingSubscription = _dbContext.EventSubscriptions.FirstOrDefault(es => es.Id == data.Id);

            if (existingSubscription == null)
            {
                throw new InvalidOperationException($"EventSubscription with ID {data.Id} not found.");
            }

            existingSubscription.SetStatus(data.Status);
            
            _dbContext.SaveChanges();

            return existingSubscription;
        }

        public List<EventSubscription> GetByEventId(long eventId)
        {
            return _dbContext.EventSubscriptions
                .Where(es => es.EventId == eventId)
                .ToList();
        }

        public EventSubscription? GetByEventIdAndUserId(long eventId, long userId)
        {
            return _dbContext.EventSubscriptions
                .SingleOrDefault(es => es.EventId == eventId && es.UserId == userId);
        }

        public EventSubscription? GetById(long id)
        {
            return _dbContext.EventSubscriptions
                .SingleOrDefault(es => es.Id == id);
        }

        public EventSubscription? GetByIdWithEvent(long id)
        {
            return _dbContext.EventSubscriptions
                .Include(es => es.Event)
                .SingleOrDefault(es => es.Id == id);
        }

        public List<EventSubscription> GetByUserId(long userId)
        {
            return _dbContext.EventSubscriptions
                .Where (es => es.UserId == userId)
                .ToList ();
        }
    }
}
