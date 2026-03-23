using Analogix_Backend_App.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class RatingCreateRequestDto
    {
        [Required]
        [Range(1, long.MaxValue)]
        public required long EventId { get; set; }

        public long? TargetUserId { get; set; } // This property is used to specify the user being rated when the rating is for a player. It is optional and should only be provided when the TargetType is Player.
        [Required]
        public required RatingTargetType TargetType { get; set; }

        [Required]
        [Range(1, 5)]
        public required int Score { get; set; }
    }
}
