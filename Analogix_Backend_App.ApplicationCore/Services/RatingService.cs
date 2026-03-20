using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IEventRepository _eventRepository;

        public RatingService(IRatingRepository ratingRepository, IEventRepository eventRepository)
        {
            _ratingRepository = ratingRepository;
            _eventRepository = eventRepository;
        }


        public Rating Create(long eventId, long raterUserId, long? targetUserId, RatingTargetType targetType, int score)
        {
            // check score is between 1 and 5
            if (score < 1 || score > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 1 and 5.");
            }

            Event ev = _eventRepository.GetByIdWithSubscriptions(eventId) ?? throw new ArgumentException("Event not found.", nameof(eventId));

            // event creators cannot rate their own events
            if(targetType == RatingTargetType.Event && ev.CreatorId == raterUserId)
            {
                throw new InvalidOperationException("Event creators cannot rate their own events.");
            }

            // players cannot rate themselves

            if (targetType == RatingTargetType.Player && targetUserId == raterUserId) { 
                throw new InvalidOperationException("Users cannot rate themselves.");
            }





            Rating rating = new Rating(
            
                eventId,
                raterUserId,
                targetType,
                score,
                targetUserId
               
            );

            return _ratingRepository.Create(rating);
        }

        public (double averageScore, int totalRatings) GetEventRatingSummary(long eventId)
        {
            throw new NotImplementedException();
        }

        public (double averageScore, int totalRatings) GetUserRatingSummary(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
