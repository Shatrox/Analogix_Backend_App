using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IEventSubscriptionRepository
    {

            EventSubscription Create(EventSubscription data); // Create a new subscription
            EventSubscription Update(EventSubscription data); // Update an existing subscription (e.g., change status from pending to accepted)
            EventSubscription? GetById(long id); // Get a subscription by its ID
            EventSubscription? GetByIdWithEvent(long id); // Get a subscription by its ID along with the associated event   
            List<EventSubscription> GetByEventId(long eventId); // Get all subscriptions for a specific event
            EventSubscription? GetByEventIdAndUserId(long eventId, long userId); // Check if a user is already subscribed to an event
            List<EventSubscription> GetByUserId(long userId);

    }
}
