using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;

        public RatingService(IRatingRepository ratingRepository, IEventRepository eventRepository, IUserRepository userRepository)
        {
            _ratingRepository = ratingRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }


        public Rating Create(long eventId, long raterUserId, long? targetUserId, RatingTargetType targetType, int score)
        {

            // check score is between 1 and 5
            if (score < 1 || score > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 1 and 5.");
            }

            Event ev = _eventRepository.GetByIdWithSubscriptions(eventId) ?? throw new ArgumentException("Event not found.", nameof(eventId));

            // check that rater is an accepted participant of the event
            bool isRaterParticipant = ev.Subscriptions.Any(s => 
                s.UserId == raterUserId &&
                s.Status == SubscriptionStatus.Accepted);

            // allows owner of the event to rate
            bool isCreator = ev.CreatorId == raterUserId;


            if (!isRaterParticipant && !isCreator)
            {
                throw new InvalidOperationException("Rater must be accepted at the event to be able to rate players and events");
            }

            // event creators cannot rate their own events
            if (targetType == RatingTargetType.Event && ev.CreatorId == raterUserId)
            {
                throw new InvalidOperationException("Event creators cannot rate their own events.");
            }

            // players cannot rate themselves

            if (targetType == RatingTargetType.Player && targetUserId == raterUserId) { 
                throw new InvalidOperationException("Users cannot rate themselves.");
            }

            // if rating a player, check that the target user is an accepted participant of the event
            if (targetType == RatingTargetType.Player)
            {
                bool isTargetParticipant = ev.Subscriptions.Any(s =>
                    s.UserId == targetUserId &&
                    s.Status == SubscriptionStatus.Accepted);
                if (!isTargetParticipant)
                {
                    throw new InvalidOperationException("Target user must be an accepted participant of the event to be able to be rated.");
                }
            }

            //prevent duplicate ratings - a user can only rate a specific target (event or player) once per event
            var alreadyRated = _ratingRepository.GetByRaterAndTarget(eventId, raterUserId, targetType, targetUserId);
            if (alreadyRated != null)
            {
                throw new InvalidOperationException("User has already rated this target for the event.");
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
            if (eventId <= 0) 
            {  
                throw new ArgumentOutOfRangeException(nameof(eventId), "Event ID must be a positive number.");
            }

            Event result = _eventRepository.GetById(eventId) ?? throw new InvalidOperationException("Event not found");

            List<Rating> ratings = _ratingRepository.GetRatingsForEvent(eventId);
            int totalRatings = ratings.Count;

            // if there are no ratings, return 0 for average score to avoid division by zero
            if (ratings.Count == 0)
            {
                return (0, 0);
            }

            // calculate average score and round to 2 decimal places
            double averageScore = Math.Round(ratings.Average(r => r.Score), 2, MidpointRounding.AwayFromZero);
           
            return (averageScore, totalRatings);
        }

        public (double averageScore, int totalRatings) GetUserRatingSummary(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be a positive number.");
            }

            User result = _userRepository.GetById(userId) ?? throw new InvalidOperationException("User not found.");

            List<Rating> ratings = _ratingRepository.GetRatingsForUser(userId);
            int totalRatings = ratings.Count;

            // if there are no ratings, return 0 for average score to avoid division by zero
            if (ratings.Count == 0)
            {
                return (0, 0);
            }

            // calculate average score and round to 2 decimal places
            double averageScore = Math.Round(ratings.Average(r => r.Score), 2, MidpointRounding.AwayFromZero);

            return (averageScore, totalRatings);
        }

        public List<Rating> GetRatingsByRaterForEvent(long eventId, long raterUserId)
        {
            return _ratingRepository.GetByRaterForEvent(eventId, raterUserId);
        }
    }
}
