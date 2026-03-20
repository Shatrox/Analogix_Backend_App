using Analogix_Backend_App.Domain.Enums;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class EventSubscriptionResponseDto
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long UserId { get; set; }
        public SubscriptionStatus Status { get; set; }  
        public DateTime CreatedAt { get; set; }
        public DateTime? ResponseAt { get; set; }    
    }
}
