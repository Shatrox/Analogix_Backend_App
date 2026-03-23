namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class EventFaqResponseDto
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long AuthorUserId { get; set; }
        public string Question { get; set; } = default!;
        public DateTime AskedAtUtc { get; set; }
        public string? Answer { get; set; } 
        public long? AnsweredUserId { get; set; }
        public DateTime? AnsweredAtUtc { get; set; }

    }
}
