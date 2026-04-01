using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IEventRepository
    {

        Event Create(Event data, IReadOnlyCollection<string>? gameTags);
        Event Update(Event data, IReadOnlyCollection<string>? gameTags);
        void Delete(Event data);
        Event? GetById(long id);
        List<Event> GetMyEvents(long userId);
        List<Event> GetEventsUserParticipated(long userId);
        Event? GetByIdWithSubscriptions(long id);
        List<Event> GetAll(string? gameTag = null);
        List<Event> GetEventsNotOwnedByUser(long userId);
        Event TransferOwnership(long eventId, long newOwnerId);

    }
}
