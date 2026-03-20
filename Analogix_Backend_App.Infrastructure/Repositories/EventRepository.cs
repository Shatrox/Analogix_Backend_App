using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
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


        public Event Create(Event data) // allow to create an event
        {
            var result = _dbContext.Events.Add(data);

            _dbContext.SaveChanges();

            return result.Entity;
        }

        public Event Update(Event data)
        {
            var existingEvent = _dbContext.Events.FirstOrDefault(e => e.Id == data.Id); // find the existing event by ID

            if (existingEvent == null) 
            {
                throw new InvalidOperationException("Event not found");
            }

            existingEvent.Title = data.Title;
            existingEvent.Description = data.Description;
            existingEvent.Location = data.Location;
            existingEvent.StartDate = data.StartDate;
            existingEvent.EndDate = data.EndDate;



            _dbContext.SaveChanges();

            return existingEvent;
        }

        public void Delete(Event data)
        {
            _dbContext.Events.Remove(data);
            _dbContext.SaveChanges();

        }

        public List<Event> GetAll()
        {
            return _dbContext.Events
                .OrderBy(e => e.StartDate)
                .ToList();
        }

        public Event? GetById(long id)
        {
           return _dbContext.Events.SingleOrDefault(e => e.Id == id);

        }

        public Event? GetByIdWithSubscriptions(long id) 
        {
            return _dbContext.Events
                .Include(e => e.Subscriptions)
                .SingleOrDefault(e => e.Id == id);

        }

    }
}
