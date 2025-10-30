using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Application.Services.Webhooks
{
    public interface IWebHookService
    {
        public Task SendLoginWebhookAsync(string url, object payload);
    }
}
