using Analogix_Backend_App.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class PlayerReportCreateRequestDto
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        public required long EventId { get; set; }

        [Required]
        public required long ReportedPlayerId { get; set; }

        [Required]
        public required ReasonsToReport Reason { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public required string Description { get; set; }
    }
}
