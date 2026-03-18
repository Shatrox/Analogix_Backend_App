using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IEventSubscriptionService
    {
        EventSubscription Subscribe(long userId, long eventId);
        EventSubscription Unsubscribe(long userId, long eventId); // Participant cancels their subscription
        EventSubscription SetStatus(long subscriptionId, SubscriptionStatus status); // Event creator accepts or rejects a subscription
        List<EventSubscription> GetSubscriptionsForOwner(long creatorId, long eventId); // Get all subscriptions for an event owned by the creator

    }
}
