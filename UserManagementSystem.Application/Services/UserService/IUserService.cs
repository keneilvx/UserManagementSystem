using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.DTOs.User;

namespace UserManagementSystem.Application.Services.UserService
{
    public interface IUserService
    {
        public Task<ReadUserDTO?> UpdateUser(UpdateUserDTO user);
        public Task<bool> DeleteUser(Guid guid);
        public Task<ReadUserDTO?> GetUser(Guid id);
    }
}
