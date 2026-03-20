using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class EventCreateRequestDto
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string Title { get; set; }

        [StringLength(4_000)]
        public string? Description { get; set; }
        [Required]
        [StringLength(120)]
        public required string Location { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(2, 300)]
        public required int MaxParticipants { get; set; }   
    }
}
