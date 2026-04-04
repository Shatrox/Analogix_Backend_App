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
    public class EventFaqController : ControllerBase
    {
        private readonly IEventFaqService _eventFaqService;

        public EventFaqController(IEventFaqService eventFaqService)
        {
            _eventFaqService = eventFaqService;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetByEventId(long eventId)
        {
            var eventFaq = _eventFaqService.GetByEventId(eventId);

            if (eventFaq == null)
            {
                return NotFound();
            }

            var result = _eventFaqService.GetByEventId(eventId)
                                         .Select(ToDto)
                                         .ToList();

            return Ok(result);
        }


        [HttpPost("questions")]
        public IActionResult AskQuestion(long eventId, [FromBody] EventFaqAskRequestDto dto)
        {
            if (dto == null || !ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }

            long userId = GetUserId();

            var result = _eventFaqService.AskQuestion(eventId, userId, dto.Question);

            return Ok(ToDto(result));
        }


        [HttpPost("questions/{questionId:long}/answer")]
        public IActionResult AnswerQuestion(long eventId, long questionId, [FromBody] EventFaqAnswerRequestDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long userId = GetUserId();

            var result = _eventFaqService.AnswerQuestion(eventId, questionId, userId, dto.Answer);

            return Ok(ToDto(result));
        }


        [HttpDelete("questions/{questionId:long}/delete/question")]
        public IActionResult DeleteQuestion(long eventId, long questionId)
        {
            long userId = GetUserId();

            _eventFaqService.DeleteQuestion(eventId, questionId, userId);

            return NoContent();
        }

        [HttpDelete("questions/{questionId:long}/delete/answer")]
        public IActionResult DeleteAnswer(long eventId, long questionId)
        {
            long userId = GetUserId();

            _eventFaqService.DeleteAnswer(eventId, questionId, userId);

            return NoContent();
        }

        private long GetUserId()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                throw new InvalidOperationException("User ID claim is missing or invalid.");
            }
            return userId;
        }


        private static EventFaqResponseDto ToDto(EventFaq faq)
        {
            return new EventFaqResponseDto
            {
                Id = faq.Id,
                EventId = faq.EventId,
                AuthorUserId = faq.AuthorUserId,
                AuthorUserName = faq.AuthorUser?.Username,
                Question = faq.Question,
                AskedAtUtc = faq.AskedAtUtc,
                Answer = faq.Answer,
                AnsweredUserId = faq.AnsweredUserId,
                AnsweredUserName = faq.AnsweredUser?.Username,
                AnsweredAtUtc = faq.AnsweredAtUtc
            };
        }





    }
}
