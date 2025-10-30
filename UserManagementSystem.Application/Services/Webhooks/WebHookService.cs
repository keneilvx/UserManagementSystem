using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserManagementSystem.Domain.DTOs;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Infrastructure.Logging;

namespace UserManagementSystem.Application.Services.Webhooks
{
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient _http;
        private readonly ILogger<WebhookService> _logger;

        public WebhookService(HttpClient http, ILogger<WebhookService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task SendLoginWebhookAsync(List<ReadUserDTO> users)
        {
            if (users == null || users.Count == 0)
                return;

            var url = "http://localhost:5298/api/webhook/user-logged-in"; 

            var payload = new UserLoggedInEventDTO
            {
                Event = "user_logged_in",
                Timestamp = DateTime.UtcNow,
                ActiveUsers = users
            };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Webhook returned non-success status code {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending login webhook");
            }
        }
    }

}
