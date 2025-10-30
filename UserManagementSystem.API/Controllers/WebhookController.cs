using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Domain.DTOs.User;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        [HttpPost("user-logged-in")]
        public IActionResult UserLoggedIn([FromBody] UserLoggedInEventDTO loginEvent)
        {
            if (loginEvent?.ActiveUsers == null || !loginEvent.ActiveUsers.Any())
            {
                _logger.LogWarning("No Active Users");
                return BadRequest(new { Message = "No active users provided." });
            }

   
            var thirtyMinutesAgo = DateTime.UtcNow.AddMinutes(-30);
            var recentUsers = loginEvent.ActiveUsers
                .Where(u => u.LastLoginAt >= thirtyMinutesAgo)
                .ToList();

    
            _logger.LogInformation("Webhook received at {ReceivedAt}. Event: {Event}, Timestamp: {Timestamp}, UsersCount: {UsersCount}",
                DateTime.UtcNow,
                loginEvent.Event,
                loginEvent.Timestamp,
                recentUsers.Count);

            foreach (var user in recentUsers)
            {
                _logger.LogInformation("User logged in within 30 mins: {Username} ({Email}) at {LastLoginAt}",
                    user.Username,
                    user.Email,
                    user.LastLoginAt);
            }

            return Ok(new
            {
                Message = $"{recentUsers.Count} users logged in within the last 30 minutes.",
                Users = recentUsers
            });
        }
    }
}
