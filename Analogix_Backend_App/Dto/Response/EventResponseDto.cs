namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class EventResponseDto
    {

        public long Id { get; set; }
        public long creatorId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string Location { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxParticipants { get; set; }

        // GameTags is a collection of strings representing the tags associated with the event.
        public IReadOnlyCollection<string> GameTags{ get; set; } = Array.Empty<string>(); // ensure collection is never null, even if there are no tags
    }
}
