using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IEventFaqService
    {
        EventFaq AskQuestion(long eventId, long actorUserId, string question);
        EventFaq AnswerQuestion(long eventId, long questionId, long actorUserId, string answer);
        void DeleteQuestion(long eventId, long questionId, long actorUserId);
        EventFaq DeleteAnswer(long eventId, long questionId, long actorUserId);
        List<EventFaq> GetByEventId(long eventId);
    }
}
