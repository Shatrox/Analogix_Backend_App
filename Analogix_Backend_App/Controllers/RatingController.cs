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
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        //Dependency injection
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost("rate")]
        public IActionResult Create([FromBody] RatingCreateRequestDto dto)
        {

            if (dto == null || !ModelState.IsValid)
            {

                return BadRequest(ModelState);


            }

            long raterUserId = GetUserId();

            var result = _ratingService.Create(
                dto.EventId,
                raterUserId,
                dto.TargetUserId,
                dto.TargetType,
                dto.Score
            );

            return Ok(ToDto(result));

        }

        [HttpGet("events/{eventId:long}/rating-summary")]
        [AllowAnonymous]
        public IActionResult GetEventRatingSummary(long eventId)
        {
            var (averageScore, totalRatings) = _ratingService.GetEventRatingSummary(eventId);

            return Ok(new RatingSummaryResponseDto
            {
                AverageScore = averageScore,
                TotalRatings = totalRatings
            });


        }

        [HttpGet("players/{userId:long}/rating-summary")]
        [AllowAnonymous]
        public IActionResult GetUserRatingSummary(long userId)
        {
            var (averageScore, totalRatings) = _ratingService.GetUserRatingSummary(userId);

            return Ok(new RatingSummaryResponseDto
            {
                AverageScore = averageScore,
                TotalRatings = totalRatings
            });
        }   

        // Helper method to extract user ID from claims
        private long GetUserId()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                throw new InvalidOperationException("User ID claim is missing or invalid.");
            }
            return userId;

        }

        private static RatingResponseDto ToDto(Rating rating) 
        { 
        
            return new RatingResponseDto
            {
                Id = rating.Id,
                EventId = rating.EventId,
                RaterUserId = rating.RaterUserId,
                TargetUserId = rating.TargetUserId,
                TargetType = rating.TargetType,
                Score = rating.Score,
                CreatedAt = rating.CreatedAt
            };  



        }





    }
}
