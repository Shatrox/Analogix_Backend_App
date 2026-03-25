using System.ComponentModel.DataAnnotations;

namespace Analogix_Backend_App.Presentation.WebAPI.Dto.Request
{
    public class EventTransferOwnershipRequestDto
    {
        [Required]
        public required long newOwnerId {  get; set; }
    }
}
