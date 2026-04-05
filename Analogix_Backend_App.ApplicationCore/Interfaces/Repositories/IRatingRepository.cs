using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IRatingRepository
    {
        Rating Create(Rating data);
        Rating? GetByRaterAndTarget(long eventId, long raterUserId, RatingTargetType targetType, long? targetUserId = null);
        List<Rating> GetRatingsForEvent(long eventId);
        List<Rating> GetRatingsForUser(long userId);
        List<Rating> GetByRaterForEvent(long eventId, long raterUserId);
    }
}
