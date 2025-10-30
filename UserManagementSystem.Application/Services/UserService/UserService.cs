using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Infrastructure.Context;
using UserManagementSystem.Domain.Mappings.Users;

namespace UserManagementSystem.Application.Services.UserService
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ReadUserDTO>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            var result = users.Select(x => x.ToReadDTO());

            return result;
        }
        public async Task<ReadUserDTO?> GetUser(Guid id)
        {
                var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new ReadUserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username
                })
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<ReadUserDTO?> UpdateUser(UpdateUserDTO user)
        {
            var userToUpdate = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            if (userToUpdate == null)
                return null;

            // Update fields
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.Username = user.Username;

            await _context.SaveChangesAsync();

            // Map to ReadUserDTO
            return new ReadUserDTO
            {
                Id = userToUpdate.Id,
                FirstName = userToUpdate.FirstName,
                LastName = userToUpdate.LastName,
                Email = userToUpdate.Email,
                Username = userToUpdate.Username
            };
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
