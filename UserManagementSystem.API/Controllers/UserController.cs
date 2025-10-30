using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Application.Services.AuthService;
using UserManagementSystem.Application.Services.UserService;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Mappings.Users;
using UserManagementSystem.Infrastructure.Context;
using UserManagementSystem.Infrastructure.Logging;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly GenericLogger<UserController> _logger;

        public UserController(AppDbContext context, IUserService userService, GenericLogger<UserController> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("/users")]
        public async Task<ActionResult<IEnumerable<ReadUserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public  async Task<IActionResult> GetUser(Guid id) 
        {
            var response = await _userService.GetUser(id);
            if (response == null)
            {
                _logger.LogWarning("User with id {UserId} not found", id);
                return Unauthorized("User not found");
            }

            return Ok(response);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDTO updateUser)
        {
            updateUser.Id = id;

            var updatedUser = await _userService.UpdateUser(updateUser);

            if (updatedUser == null)
            {
                _logger.LogWarning("Failed to update {UserId}", id);
                return NotFound(new { message = "User not found" });
            }
               
            return Ok(updatedUser);
        }

        [Authorize]
        [HttpDelete("{id}") ]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _userService.DeleteUser(id);

            if (!deleted)
            {
                _logger.LogWarning("Failed to delete user with id {UserId}", id);
                return NotFound(new { message = "User not found" });
            }
               
            return NoContent(); 
        }
    }
}
