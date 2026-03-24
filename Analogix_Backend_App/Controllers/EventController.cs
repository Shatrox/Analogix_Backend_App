using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Models;
using Analogix_Backend_App.Presentation.WebAPI.Dto.Request;
using Analogix_Backend_App.Presentation.WebAPI.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Analogix_Backend_App.Presentation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // This attribute indicates that the controller requires authorization. Users must be authenticated to access the endpoints defined in this controller.
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("events/create")]
        public IActionResult Create([FromBody] EventCreateRequestDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long userId = GetUserId();

            Event created = _eventService.Create(dto.Title, dto.Description, dto.Location, dto.StartDate, dto.EndDate, dto.MaxParticipants, userId, dto.GameTags);


            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));

        }


        [HttpPut("events/update/{id:long}")]
        public IActionResult Update(long id, [FromBody] EventUpdateRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            long userId = GetUserId();

            Event eventUpdated = _eventService.Update(
                id,
                dto.Title,
                dto.Description,
                dto.Location,
                dto.StartDate,
                dto.EndDate,
                dto.MaxParticipants,
                userId,
                dto.GameTags
            );

            return Ok(ToDto(eventUpdated));

        }

        [HttpDelete("events/delete/{id:long}")]

        public IActionResult DeleteEvent(long id)
        {

            long userId = GetUserId();

            _eventService.Delete(userId, id);

            return NoContent();

        }


        [HttpGet]
        [AllowAnonymous] // This attribute allows anonymous access to this specific endpoint, meaning that users do not need to be authenticated to access it.
        public IActionResult GetAll([FromQuery] string? gameTag)
        {
            // This endpoint retrieves all events, optionally filtering by a specific game tag.
            var result = _eventService.GetAll(gameTag)
                .Select(ToDto)
                .ToList();

            return Ok(result);

        }

        [HttpGet("events/{id:long}")]
        [AllowAnonymous]
        public IActionResult GetById(long id)
        {
            var result = _eventService.GetById(id);

            return Ok(ToDto(result!));

        }

        private static EventResponseDto ToDto(Event ev)
        {
            return new EventResponseDto
            {
                Id = ev.Id,
                creatorId = ev.CreatorId,
                Title = ev.Title,
                Description = ev.Description,
                Location = ev.Location,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
                MaxParticipants = ev.MaxParticipants,
                //Tags
                GameTags = ev.GameTags.Select(gt => gt.Name).OrderBy(t => t).ToList() // OrderBy(t => t) -> Lambda expression that sorts the game tags alphabetically
            };
        }


        private long GetUserId()
        {
            // This method retrieves the user ID from the claims of the authenticated user.

            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
            }


            return userId;

        }
    }
}
