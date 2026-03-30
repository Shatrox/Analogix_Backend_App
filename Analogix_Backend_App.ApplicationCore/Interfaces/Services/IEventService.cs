using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IEventService
    {
        Event Create(string title, string? description, string location, DateTime startDate, DateTime? endDate, int maxParticipants, long creatorId, IReadOnlyCollection<string>? gameTags);
        Event Update(long id, string title, string? description, string location, DateTime startDate, DateTime? endDate, int maxParticipants, long creatorId, IReadOnlyCollection<string>? gameTags);
        void Delete (long creatorId, long eventId);
        Event? GetById(long id);
        List<Event> GetEventsUserParticipated(long userId);
        List<Event> GetAll(string? gameTag = null);
        Event TransferOwnership(long eventId, long currentOwnerId, long newOwnerId);


    }
}
