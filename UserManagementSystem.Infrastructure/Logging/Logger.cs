using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Infrastructure.Logging
{
    public class GenericLogger<T>
    {
        private readonly ILogger<T> _logger;

        public GenericLogger(ILogger<T> logger) 
        {               
            _logger = logger;     
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogException(Exception ex, string message = "")
        {
            _logger.LogError(ex, message);
        }
    }
}
