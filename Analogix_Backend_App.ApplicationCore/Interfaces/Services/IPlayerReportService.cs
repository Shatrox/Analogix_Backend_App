using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IPlayerReportService
    {
        PlayerReport Create(long id, long reporterId, long eventId, long reportedPlayerId, ReasonsToReport reason, string description);
        PlayerReport Review(long reportId, long reviewerId, ReportStatus reportStatus, string? reviewNote);
        List<PlayerReport> GetReports(long reporterId);
        List<PlayerReport> GetPendingReports(long actorUserID);
    }
}
