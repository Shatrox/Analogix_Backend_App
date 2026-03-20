using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IRatingService
    {
        Rating Create(long eventId, long raterUserId, long? targetUserId, RatingTargetType targetType, int score); 

        // Tuple - allows us to return both the average score and the total number of ratings in a single method call
        (double averageScore, int totalRatings) GetEventRatingSummary(long eventId);
        (double averageScore, int totalRatings) GetUserRatingSummary(long userId);
    }
}
