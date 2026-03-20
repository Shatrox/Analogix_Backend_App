using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class EventService : IEventService
    {

        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Event Create(string title, string? description, string location, DateTime startDate, DateTime? endDate, int maxParticipants, long creatorId)
        {
            ValidateStartDate(startDate);
            ValidateMaxParticipants(maxParticipants);

            var newEvent = new Event
            (
                title,
                description,
                location,
                startDate,
                endDate,
                maxParticipants,
                creatorId
            );

            _eventRepository.Create(newEvent);

            return newEvent;
        }

        public Event Update(long id, string title, string? description, string location, DateTime startDate, DateTime? endDate, int maxParticipants, long creatorId)
        {
            ValidateStartDate(startDate);
            ValidateMaxParticipants(maxParticipants);

            var existing = _eventRepository.GetById(id) ?? throw new KeyNotFoundException($"Event with ID {id} not found.");

            if(existing.CreatorId != creatorId) {
            
                throw new UnauthorizedAccessException("You are not authorized to update this event.");
            }

            existing.Title = title;
            existing.Description = description;
            existing.Location = location;
            existing.StartDate = startDate;
            existing.EndDate = endDate;
            existing.MaxParticipants = maxParticipants;

            return _eventRepository.Update(existing);


        }

        public void Delete(long creatorId, long eventId)
        {
            var existing = _eventRepository.GetById(eventId) ?? throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            
            if (existing.CreatorId != creatorId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this event.");
            }

            _eventRepository.Delete(existing);  
        }

        public List<Event> GetAll()
        {
            return _eventRepository.GetAll();
        }

        public Event? GetById(long id)
        {
            return _eventRepository.GetById(id) ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
        }

        private static void ValidateStartDate(DateTime startDate)
        {
            if (startDate <= DateTime.UtcNow)
            {
                throw new ArgumentException("Event date must be in the future.");
            }
        }

        private static void ValidateMaxParticipants(int maxParticipants)
        {
            if (maxParticipants < 2)
            {
                throw new ArgumentException("MaxParticipants must be greater than 1.");
            }
        }

    }
}
