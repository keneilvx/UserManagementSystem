using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Infrastructure.Context;
using UserManagementSystem.Domain.Mappings.Users;
using UserManagementSystem.Application.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using UserManagementSystem.Application.Services.UserService;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(AppDbContext context, IAuthService authService, IUserService userService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadUserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            var result = users.Select(x => x.ToReadDTO());

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public  async Task<IActionResult> AuthenicateUser([FromBody] LoginRequestDTO login) 
        {
            var response = await _authService.LoginAsync(login);
            if (response == null)
            {
                return Unauthorized("Invalid Email or Password.");
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
                return NotFound(new { message = "User not found" });

            return Ok(updatedUser);
        }

        [Authorize]
        [HttpDelete("{id}") ]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _userService.DeleteUser(id);

            if (!deleted)
                return NotFound(new { message = "User not found" });

            return NoContent(); 
        }
    }
}
