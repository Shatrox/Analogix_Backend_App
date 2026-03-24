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
    [Authorize] // This attribute indicates that all actions in this controller require the user to be authenticated. It ensures that only authenticated users can access the endpoints defined in this controller.
    public class PlayerProfileController : ControllerBase
    {
        private readonly IPlayerProfileService _playerProfileService;    

        public PlayerProfileController(IPlayerProfileService playerProfileService)
        {
            _playerProfileService = playerProfileService;
        }

        [HttpPost("create/profile")]
        public IActionResult Create([FromBody] PlayerProfileRequestDto dto)
        {
            if (dto == null || !ModelState.IsValid) 
            { 
            
                return BadRequest(ModelState);

            }

            // Take the userId from the token
            long userId = GetUserId();

            var result = _playerProfileService.Create(
                userId,
                dto.Biography,
                dto.FavoriteGames,
                dto.MasteryLevel,
                dto.FavoriteGameTags
            );

            return Ok(ToDto(result)); 

        }

        

        [HttpPut("update/profile")]
        public IActionResult Update([FromBody] PlayerProfileRequestDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Take the userId from the token
            long userId = GetUserId();
            var result = _playerProfileService.Update(
                userId,
                dto.Biography,
                dto.FavoriteGames,
                dto.MasteryLevel,
                dto.FavoriteGameTags
            );

            return Ok(ToDto(result));
        }


        private static PlayerProfileResponseDto ToDto(PlayerProfile profile)    
        {
            return new PlayerProfileResponseDto
            {
                Biography = profile.Biography,
                FavoriteGames = profile.FavoriteGames,
                MasteryLevel = profile.MasteryLevel,
                UserId = profile.UserId,
                FavoriteGameTags = profile.FavoriteGameTags.Select(t => t.Name).ToList()
            };
        }

        private long GetUserId() // Allows us to retrieve the user ID from the claims of the authenticated user.
        {

            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                throw new InvalidOperationException("User ID claim is missing or invalid.");
            }


            return userId; // If the claim is successfully retrieved and parsed as a long, this line returns the user ID as a long value.

        }
    }
}
