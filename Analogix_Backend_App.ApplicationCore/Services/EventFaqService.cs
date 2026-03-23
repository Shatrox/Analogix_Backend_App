using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class EventFaqService : IEventFaqService
    {
        private readonly IEventFaqRepository _eventFaqRepository;
        private readonly IEventRepository _eventRepository;

        public EventFaqService(IEventFaqRepository eventFaqRepository, IEventRepository eventRepository) 
        {
        
            _eventFaqRepository = eventFaqRepository;
            _eventRepository = eventRepository;
 
        }

        public EventFaq AskQuestion(long eventId, long actorUserId, string question)
        {
            Event ev = _eventRepository.GetByIdWithSubscriptions(eventId) ?? throw new Exception("Event not found.");

            bool isOwner = ev.CreatorId == actorUserId;
            bool isSubscribed = ev.Subscriptions.Any(s => s.UserId == actorUserId && s.Status == SubscriptionStatus.Accepted);

            if (isOwner || !isSubscribed) 
            { 
                throw new UnauthorizedAccessException("Only accepted subscribers can ask questions.");
            }

            EventFaq faq = new EventFaq
            (
                eventId,
                actorUserId,
                question
            );

            return _eventFaqRepository.Create(faq);
        }

        public void DeleteQuestion(long eventId, long questionId, long actorUserId)
        {
            EventFaq faq = _eventFaqRepository.GetById(questionId) ?? throw new Exception("Question not found.");

            if(faq.EventId != eventId)
            {
                throw new Exception("Question does not belong to the event.");
            }

            if(faq.AuthorUserId != actorUserId)
            {
                throw new UnauthorizedAccessException("Only question author can delete the question.");
            }

            _eventFaqRepository.Delete(faq);
        }

        public EventFaq AnswerQuestion(long eventId, long questionId, long actorUserId, string answer)
        {
            Event ev = _eventRepository.GetById(eventId) ?? throw new Exception("Event not found");
            bool isOwner = ev.CreatorId == actorUserId;

            if(!isOwner)
            {
                throw new UnauthorizedAccessException("Only event owner can answer questions");
            }

            // checks if question exists
            EventFaq faq = _eventFaqRepository.GetById(questionId) ?? throw new Exception("Question not found");

            // checks if question belongs to the event
            if(faq.EventId != eventId)
            {
                throw new Exception("Question does not belong to the event");
            }

            faq.AddAnswer(actorUserId, answer);
            _eventFaqRepository.Update(faq);

            return faq;
        }

        public EventFaq DeleteAnswer(long eventId, long questionId, long actorUserId)
        {
            EventFaq faq = _eventFaqRepository.GetById(questionId) ?? throw new Exception("Question not found.");

            if(faq.EventId != eventId)
            {
                throw new Exception("Answer does not belong to the event.");
            }

            if(faq.AnsweredUserId != actorUserId)
            {
                throw new UnauthorizedAccessException("Only answer author can delete the answer.");
            }

            if(faq.Answer == null)
            {
                throw new Exception("No answer to delete.");
            }

            faq.RemoveAnswer();
            return _eventFaqRepository.Update(faq);
        }

        public List<EventFaq> GetByEventId(long eventId)
        {
            var ev = _eventRepository.GetById(eventId) 
                ?? throw new KeyNotFoundException(" Event not found");

            return _eventFaqRepository.GetByEventId(eventId);
        }
    }
}
