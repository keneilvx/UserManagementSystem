using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Domain.DTOs.User
{
    public class UserLoggedInEventDTO
    {
        public string Event { get; set; }
        public DateTime Timestamp { get; set; }
        public List<ReadUserDTO>? ActiveUsers { get; set; }

    }
}
