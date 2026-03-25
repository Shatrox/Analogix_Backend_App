using Analogix_Backend_App.ApplicationCore.Interfaces.Repositories;
using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Services
{
    public class PlayerReportService : IPlayerReportService
    {
        private readonly IPlayerReportRepository _playerReportRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;

        public PlayerReportService(IPlayerReportRepository playerReportRepository, IEventRepository eventRepository, IUserRepository userRepository)
        {
            _playerReportRepository = playerReportRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public PlayerReport Create(long id, long reporterId, long eventId, long reportedPlayerId, ReasonsToReport reason, string description)
        {
            Event ev = _eventRepository.GetByIdWithSubscriptions(eventId) ?? throw new Exception("Event not found");

            bool isReporterParticipant = ev.CreatorId == reporterId || ev.Subscriptions.Any(s => s.UserId == reporterId && s.Status == SubscriptionStatus.Accepted);

            if(!isReporterParticipant)
                throw new UnauthorizedAccessException("Reporter must be a participant of the event");

            bool isReportedParticipant = ev.CreatorId == reportedPlayerId || ev.Subscriptions.Any(s => s.UserId == reportedPlayerId && s.Status == SubscriptionStatus.Accepted);

            if(!isReportedParticipant)
                throw new UnauthorizedAccessException("Reported player must be a participant of the event");

            PlayerReport? isAlreadyReported = _playerReportRepository.GetOpenReportByEventAndPlayer(eventId, reporterId, reportedPlayerId);
            if(isAlreadyReported != null)
                throw new InvalidOperationException("A report for this player in this event is already open");

            PlayerReport report = new PlayerReport
            (
                reporterId,
                eventId,
                reportedPlayerId,
                reason,
                description
            );

            return _playerReportRepository.Create(report);
        }

        public List<PlayerReport> GetPendingReports(long actorUserID)
        {
            
            User actor = _userRepository.GetById(actorUserID) ?? throw new KeyNotFoundException("User not found");

            if(actor.Role != UserRoles.Admin) 
            {
                throw new UnauthorizedAccessException("Only admins can view pending reports");
            }

            return _playerReportRepository.GetByStatus(ReportStatus.Pending);

        }

        public List<PlayerReport> GetReports(long reporterId)
        {
            if(reporterId <= 0) 
            {
                throw new ArgumentException("Invalid reporter ID", nameof(reporterId));
            }
                

            return _playerReportRepository.GetByReporterId(reporterId);
        }

        public PlayerReport Review(long reportId, long reviewerId, ReportStatus reportStatus, string? reviewNote)
        {
            User reviewer = _userRepository.GetById(reviewerId) ?? throw new UnauthorizedAccessException("Reviewer not found");

            if (reviewer.Role != UserRoles.Admin) 
            {
                throw new UnauthorizedAccessException("Only admins can review reports");
            }

            PlayerReport report = _playerReportRepository.GetById(reportId) ?? throw new KeyNotFoundException("Report not found");

            report.ReportReview(reviewerId, reportStatus, reviewNote);

            return _playerReportRepository.Update(report);
        }
    }
}
