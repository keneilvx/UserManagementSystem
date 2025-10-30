using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Application.Services.AuthService
{
    public interface IAuthService
    {
        public Task<Token?> LoginAsync(LoginRequestDTO request);

        public Task<User?> RegisterAsync(CreateUserDTO request);

    }
}
