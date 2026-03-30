using Analogix_Backend_App.ApplicationCore.Interfaces.Services;
using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
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
    public class EventSubscriptionController : ControllerBase
    {
        private readonly IEventSubscriptionService _eventSubscriptionService;

        public EventSubscriptionController(IEventSubscriptionService eventSubscriptionService)
        {
            _eventSubscriptionService = eventSubscriptionService;
        }


        [HttpPost("event/{eventId}/subscribe")]
        public IActionResult Subscribe(long eventId) 
        { 
        
            var userId = GetUserId();

            EventSubscription subscription = _eventSubscriptionService.Subscribe(eventId, userId);

            return Ok(ToDto(subscription));

        }

        [HttpPost("event/{eventId}/unsubscribe")]
        public IActionResult Unsubscribe(long eventId)
        {
            var userId = GetUserId();
            EventSubscription subscription = _eventSubscriptionService.Unsubscribe(userId, eventId);
            return Ok(ToDto(subscription));
        }

        // This endpoint allows the event owner to retrieve all subscriptions for a specific event they own.
        [HttpGet("event/{eventId}/subscriptions")]
        public IActionResult GetSubscriptionsForOwner(long eventId)
        {
            var userId = GetUserId();
            List<EventSubscription> subscriptions = _eventSubscriptionService.GetSubscriptionsForOwner(userId, eventId);
            return Ok(subscriptions
                .Select(ToDto)
                .ToList());
        }

        [HttpGet("my-subscriptions")]
        public IActionResult GetMySubscriptions() 
        {

            var userId = GetUserId();
            List<EventSubscription> subscriptions = _eventSubscriptionService.GetSubscriptionsForUser(userId);

            return Ok(subscriptions.Select(ToDto).ToList());
        
        
        }

        // This endpoint allows the event owner to accept a subscription request from a user.
        [HttpPatch("subscription/{subscriptionId}/accept")]
        public IActionResult Accept(long subscriptionId) 
        { 
        
            var userId = GetUserId();
            
            EventSubscription subscription = _eventSubscriptionService.SetStatus(subscriptionId, userId, SubscriptionStatus.Accepted);

            return Ok(ToDto(subscription));

        }

        // This endpoint allows the event owner to refuse a subscription request from a user.
        [HttpPatch("subscription/{subscriptionId}/refuse")]
        public IActionResult Refuse(long subscriptionId)
        {
            var userId = GetUserId();
            EventSubscription subscription = _eventSubscriptionService.SetStatus(subscriptionId, userId, SubscriptionStatus.Refused);
            return Ok(ToDto(subscription));
        }


        // This method converts an EventSubscription domain model to an EventSubscriptionResponseDto, which is a data transfer object used for sending subscription information
        private static EventSubscriptionResponseDto ToDto(EventSubscription sub)
        {
            return new EventSubscriptionResponseDto
            {
                Id = sub.Id,
                EventId = sub.EventId,
                UserId = sub.UserId,
                Status = sub.Status,
                CreatedAt = sub.CreatedAt,
                ResponseAt = sub.ResponseAt,
            };
        }

        // Used to retrieve the user ID from the claims of the authenticated user. User must be authenticated to access the endpoints in this controller.
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
