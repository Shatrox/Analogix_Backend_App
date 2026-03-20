using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class RatingRepository : IRatingRepository
    {

        private readonly AppDbContext _dbContext;

        public RatingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Rating Create(Rating data)
        {
            var result = _dbContext.Ratings.Add(data);

            _dbContext.SaveChanges();

            return result.Entity;
        }

        public Rating? GetByRaterAndTarget(long eventId, long raterUserId, RatingTargetType targetType, long? targetUserId = null)
        {
            return _dbContext.Ratings
                .SingleOrDefault(r => r.EventId == eventId &&
                    r.RaterUserId == raterUserId &&
                    r.TargetType == targetType &&
                    r.TargetUserId == targetUserId


                );
        }

        public List<Rating> GetRatingsForEvent(long eventId)
        {
            return _dbContext.Ratings
                .Where(r => r.EventId == eventId)
                .ToList();
        }

        public List<Rating> GetRatingsForUser(long userId)
        {
            return _dbContext.Ratings
                .Where(r => r.TargetType == RatingTargetType.Player && r.TargetUserId == userId)
                .ToList();
        }
    }
}
