using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class EventSubscriptionService : IEventSubscriptionService
    {
        private readonly IEventSubscriptionRepository _eventSubscriptionRepository;
        private readonly IEventRepository _eventRepository;

        public EventSubscriptionService(IEventSubscriptionRepository eventSubscriptionRepository, IEventRepository eventRepository)
        {
            _eventSubscriptionRepository = eventSubscriptionRepository;
            _eventRepository = eventRepository;
        }

        public EventSubscription Subscribe(long eventId, long userId) 
        {


            Event ev = _eventRepository.GetByIdWithSubscriptions(eventId) ?? throw new KeyNotFoundException($"Event with ID {eventId} not found.");


            // Verifies if you have already subscribed or if you were refused already
            var alreadysubscribed = _eventSubscriptionRepository.GetByEventIdAndUserId(eventId, userId);
            if (alreadysubscribed is not null && alreadysubscribed.Status!= SubscriptionStatus.Refused)
            {
                throw new InvalidOperationException("You are already subscribed to this event.");
            } 

            int acceptedCount = ev.Subscriptions.Count(s => s.Status == SubscriptionStatus.Accepted);

            if(acceptedCount >= ev.MaxParticipants) 
            {

                throw new InvalidOperationException("The event is already full.");
            
            }

            // Cannot subscribe to your own event
            if (ev.CreatorId == userId) { 
            
                throw new InvalidOperationException("You cannot subscribe to your own event.");

            }

            var subscriptionAdd = new EventSubscription(eventId, userId);
            
            return _eventSubscriptionRepository.Create(subscriptionAdd);

        }

        // Allows the user to unsubscribe from an event, but it doesn't delete the subscription, it just changes the status to Deleted. 
        public EventSubscription Unsubscribe(long userId, long eventId) 
        {

            var subscription = _eventSubscriptionRepository.GetByEventIdAndUserId(eventId, userId) ?? throw new KeyNotFoundException("Subscription not found");

            subscription.SetStatus(SubscriptionStatus.Deleted);

            return _eventSubscriptionRepository.Update(subscription);
        
        
        }

        public List<EventSubscription> GetSubscriptionsForOwner(long creatorId, long eventId)
        {
            var ev = _eventRepository.GetById(eventId) ?? throw new KeyNotFoundException($"Event with ID {eventId} not found.");

            if (ev.CreatorId != creatorId)
            {
                throw new UnauthorizedAccessException("You are not the owner of this event.");
            }

            return _eventSubscriptionRepository.GetByEventId(eventId);
        }

        public EventSubscription SetStatus(long subscriptionId, long creatorId, SubscriptionStatus status)
        {
            if (status is  not SubscriptionStatus.Accepted and not SubscriptionStatus.Refused)
            {
                throw new ArgumentException("Invalid status. Only Accepted or Refused are allowed.");
            }

            var subscription = _eventSubscriptionRepository.GetByIdWithEvent(subscriptionId) ?? throw new KeyNotFoundException("Subscription not found");

            if(subscription.Event.CreatorId != creatorId)
            {
                throw new UnauthorizedAccessException("You are not authorized to manage this subscription.");
            }

            // If the owner tries to accept another player and the event is full already, it throws an exception.

            if (status == SubscriptionStatus.Accepted) 
            { 
            
                var ev = _eventRepository.GetByIdWithSubscriptions(subscription.EventId) ?? throw new KeyNotFoundException($"Event with ID {subscription.EventId} not found.");

                int acceptedCount = ev.Subscriptions.Count(s => s.Status == SubscriptionStatus.Accepted);

                if (acceptedCount >= ev.MaxParticipants)
                {
                    throw new InvalidOperationException("The event is already full.");

                }

            }

            //If all checks are passed, it accepts the subscription.
            subscription.SetStatus(status);

            //And updates the subscription in the database.
            return _eventSubscriptionRepository.Update(subscription);






        }



    }
}
