using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class EventFaqRepository : IEventFaqRepository
    {
        private readonly AppDbContext _dbContext;

        public EventFaqRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public EventFaq Create(EventFaq data)
        {
            var result = _dbContext.EventFaqs.Add(data);
            _dbContext.SaveChanges();

            return result.Entity;
        }

        public EventFaq Update(EventFaq data)
        {
            var existingFaq = _dbContext.EventFaqs.SingleOrDefault(f => f.Id == data.Id);

            if (existingFaq is null)
            {
                throw new InvalidOperationException($"Event FAQ not found.");
            }

            existingFaq.Answer = data.Answer;
            existingFaq.AnsweredUserId = data.AnsweredUserId;
            existingFaq.AnsweredAtUtc = data.AnsweredAtUtc;

            _dbContext.SaveChanges();

            return existingFaq;
        }

        public void Delete(EventFaq data)
        {
            _dbContext.EventFaqs.Remove(data);
            _dbContext.SaveChanges();
        }

        public List<EventFaq> GetByEventId(long eventId)
        {
            return _dbContext.EventFaqs.Where(f => f.EventId == eventId)
                                       .OrderByDescending(f => f.AskedAtUtc)
                                       .ToList();
        }

        public EventFaq? GetById(long id)
        {
            return _dbContext.EventFaqs.SingleOrDefault(f => f.Id == id);
        }

        
    }
}
