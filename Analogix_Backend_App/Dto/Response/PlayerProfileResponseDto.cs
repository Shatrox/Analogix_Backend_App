using Analogix_Backend_App.Domain.Enums;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Response
{
    public class PlayerProfileResponseDto
    {
        public long Id { get; set; }
        public string? Biography { get; set; }
        public string FavoriteGames { get; set; } = default!;
        public MasteryLevel MasteryLevel { get; set; }
        public long UserId { get; set; }
        public List<string> FavoriteGameTags { get; set; } = new ();
    }
}
