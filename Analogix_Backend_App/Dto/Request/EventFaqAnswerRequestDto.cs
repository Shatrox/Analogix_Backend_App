using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class EventFaqAnswerRequestDto
    {
        [Required]
        [StringLength(1000)]
        public required string Answer { get; set; }
    }
}
