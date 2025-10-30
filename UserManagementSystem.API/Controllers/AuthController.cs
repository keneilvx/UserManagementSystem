using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Application.Services.AuthService;
using UserManagementSystem.Application.Services.Webhooks;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Mappings.Users;
using UserManagementSystem.Infrastructure.Context;
using UserManagementSystem.Infrastructure.Logging;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IWebHookService _webHookService;
        private readonly string _webhookUrl = "https://localhost:7175/api/webhook/login";
        private readonly GenericLogger<AuthController> _logger;

        public AuthController(AppDbContext context, IAuthService authService, WebHookService webHookService, GenericLogger<AuthController> logger)
        {
            _context = context;
            _authService = authService;
            _webHookService = webHookService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
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
