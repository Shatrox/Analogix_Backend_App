using Analogix_Backend_App.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class PlayerReportReviewRequestDto
    {
        [Required]
        public required ReportStatus NewStatus { get; set; }

        [MaxLength(2000)]
        public string? ReviewNote { get; set; }
    }
}
