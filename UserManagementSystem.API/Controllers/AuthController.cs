using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using UserManagementSystem.Application.Services.AuthService;
using UserManagementSystem.Application.Services.Webhooks;
using UserManagementSystem.Domain.DTOs;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Mappings.Users;
using UserManagementSystem.Infrastructure.Context;
using UserManagementSystem.Infrastructure.Logging;
using UserManagementSystem.Infrastructure.Migrations;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;    
        private readonly GenericLogger<AuthController> _logger;

        public AuthController( 
            IAuthService authService,
            GenericLogger<AuthController> logger
            )
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
                _logger.LogWarning("Failed create the user with {Request}", Request);
                return BadRequest("Username already exists.");
            }

            return Ok(new { user.Id, user.Username, user.Email });
        }

        [HttpPost("login")]
        public  async Task<IActionResult> AuthenicateUser([FromBody] LoginRequestDTO login) 
        {
            var response = await _authService.LoginAsync(login);
            if (response == null)
            {
                _logger.LogWarning("Failed user login attempt from {Email}", login.Email);
                return Unauthorized("Invalid Email or Password.");
            }

            _logger.LogInformation("Login successful for {Email}", login.Email);

            return Ok(response);
        }      

    }
}
