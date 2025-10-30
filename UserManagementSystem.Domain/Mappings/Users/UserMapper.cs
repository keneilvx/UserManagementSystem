using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Domain.Mappings.Users
{
    public static class UserMapper
    {
     
        public static ReadUserDTO ToReadDTO(this User user) {

            var dto = new ReadUserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
            };

            return dto;
        
        }
        
    }
}
