using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IPlayerReportRepository
    {
        PlayerReport Create(PlayerReport data);
        PlayerReport Update(PlayerReport data);
        PlayerReport? GetById(long id);
        List<PlayerReport> GetByReporterId(long reporterId);
        List<PlayerReport> GetByStatus(ReportStatus status);
        PlayerReport? GetOpenReportByEventAndPlayer(long eventId, long reporterId, long reportedPlayerId);

    }
}
