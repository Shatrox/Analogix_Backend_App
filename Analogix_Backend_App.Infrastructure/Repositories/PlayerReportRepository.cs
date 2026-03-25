using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Repositories
{
    public class PlayerReportRepository : IPlayerReportRepository
    {

        private readonly AppDbContext _dbContext;

        public PlayerReportRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PlayerReport Create(PlayerReport data)
        {
            var result = _dbContext.PlayerReports.Add(data);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public PlayerReport Update(PlayerReport data)
        {
            var existing = _dbContext.PlayerReports.SingleOrDefault(r => r.Id == data.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Report not found");
            }

            existing.ReportStatus = data.ReportStatus;
            existing.ReviewerId = data.ReviewerId;
            existing.ReviewedAtUtc = data.ReviewedAtUtc;
            existing.ReviewNote = data.ReviewNote;

            _dbContext.SaveChanges();
            return existing;
        }

        public PlayerReport? GetById(long id)
        {
            return _dbContext.PlayerReports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedPlayer)
                .Include(r => r.Event)
                .Include(r => r.Reviewer)
                .SingleOrDefault(r => r.Id == id);
                
        }

        public List<PlayerReport> GetByReporterId(long reporterId)
        {
            return _dbContext.PlayerReports
                .Where(r => r.ReporterId == reporterId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public List<PlayerReport> GetByStatus(ReportStatus status)
        {
            return _dbContext.PlayerReports
                .Where(r => r.ReportStatus == status) 
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public PlayerReport? GetOpenReportByEventAndPlayer(long eventId, long reporterId, long reportedPlayerId)
        {
            return _dbContext.PlayerReports.SingleOrDefault(r => 
                r.EventId == eventId && 
                r.ReporterId == reporterId && 
                r.ReportedPlayerId == reportedPlayerId && 
                (r.ReportStatus == ReportStatus.Pending || r.ReportStatus == ReportStatus.InReview));
        }

        
    }
}
