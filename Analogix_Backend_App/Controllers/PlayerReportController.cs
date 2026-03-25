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
    public class PlayerReportController : ControllerBase
    {
        private readonly IPlayerReportService _playerReportService;

        public PlayerReportController(IPlayerReportService playerReportService)
        {
            _playerReportService = playerReportService;
        }

        [HttpPost("create/report")]
        public IActionResult Create([FromBody] PlayerReportCreateRequestDto dto) 
        { 
            if(dto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long userId = GetUserId();

            PlayerReport report = _playerReportService.Create
            (
                dto.Id,
                userId,
                dto.EventId,
                dto.ReportedPlayerId,
                dto.Reason,
                dto.Description
            );

            return Ok(ToDto(report));

        }

        [HttpGet("my-reports")]
        public IActionResult GetMyReports()
        {
            long userId = GetUserId();
            var result = _playerReportService.GetReports(userId);
            return Ok(result);  
        }


        [HttpGet("pending-reports")]
        public IActionResult GetPendingReports() 
        { 
            long userId = GetUserId();
            var result = _playerReportService.GetPendingReports(userId).Select(ToDto).ToList(); 
            return Ok(result);
        }

        [HttpPatch("{reportId:long}/review")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult ReviewReport(long reportId, [FromBody] PlayerReportReviewRequestDto dto)
        {
            if (dto == null || !ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            long userId = GetUserId();

            PlayerReport reviewed = _playerReportService.Review(reportId, userId, dto.NewStatus, dto.ReviewNote);

            return Ok(ToDto(reviewed));
        }

        private long GetUserId()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
            }

            return userId;
        }

        private static PlayerReportResponseDto ToDto(PlayerReport report)
        {
            return new PlayerReportResponseDto
            {
                Id = report.Id,
                ReporterId = report.ReporterId,
                EventId = report.EventId,
                ReportedPlayerId = report.ReportedPlayerId,
                Reason = report.Reason,
                Description = report.Description,
                Status = report.ReportStatus,
                CreatedAt = report.CreatedAt,
                ReviewerId = report.ReviewerId,
                ReviewedAtUtc = report.ReviewedAtUtc,
                ReviewNote = report.ReviewNote
            };
        }

    }
}
