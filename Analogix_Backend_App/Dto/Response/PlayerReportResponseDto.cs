using Analogix_Backend_App.Domain.Enums;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class PlayerReportResponseDto
    {
        public long Id { get; set; }
        public long ReporterId { get; set; }
        public long EventId { get; set; }
        public long ReportedPlayerId { get; set; }
        public ReasonsToReport Reason { get; set; }
        public string Description { get; set; } = default!;
        public ReportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? ReviewerId { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }    
        public string? ReviewNote { get; set; }


    }
}
