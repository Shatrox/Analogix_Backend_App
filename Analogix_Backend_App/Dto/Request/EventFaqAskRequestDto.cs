using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class EventFaqAskRequestDto
    {
        [Required]
        [StringLength(1000)]
        public required string Question { get; set; }
    }
}
