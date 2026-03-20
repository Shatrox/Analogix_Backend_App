using Analogix_Backend_App.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class PlayerProfileRequestDto
    {
        [StringLength(5_000)]
        public string? Biography { get; set; }
        [Required]
        [StringLength(255)]
        public required string FavoriteGames { get; set; }
        [Required]
        public required MasteryLevel MasteryLevel { get; set; }


    }
}
