using Analogix_Backend_App.Domain.Enums;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class RatingResponseDto
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long RaterUserId { get; set; }
        public long? TargetUserId { get; set; }
        public RatingTargetType TargetType { get; set; }    
        public int Score { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
