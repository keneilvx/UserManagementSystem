using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Application.Services.Webhooks
{
    public class WebHookService : IWebHookService
    {
        private readonly HttpClient _http;

        public WebHookService(HttpClient http)
        {
            _http = http;
        }

        public async Task SendLoginWebhookAsync(string url, object payload)
        {
            await _http.PostAsJsonAsync(url, payload);
        }
    }
}
