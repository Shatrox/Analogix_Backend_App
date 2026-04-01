using Analogix_Backend_App.Domain.Enums;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class EventSubscriptionResponseDto
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? CreatorName { get; set; }
        public SubscriptionStatus Status { get; set; }  
        public DateTime CreatedAt { get; set; }
        public DateTime? ResponseAt { get; set; } 
        public string EventTitle { get; set; } = default!;
        public string EventLocation { get; set; } = default!;

    }
}
