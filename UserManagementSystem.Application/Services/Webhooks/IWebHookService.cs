using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.DTOs.User;

namespace UserManagementSystem.Application.Services.Webhooks
{
    public interface IWebhookService
    {
        public Task SendLoginWebhookAsync(List<ReadUserDTO> users);
    }
}
