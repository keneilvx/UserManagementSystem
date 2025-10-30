using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementSystem.Domain.DTOs;
using UserManagementSystem.Domain.DTOs.User;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Infrastructure.Context;


namespace UserManagementSystem.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(AppDbContext context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }
   
        public async Task<User?> RegisterAsync(CreateUserDTO request)
        {
            if (_context.Users.Any(u => u.Username == request.Email || u.Username == request.Username))
            {
                return null; 
            }

            var user = new User
            {
                Email = request.Email,
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,  
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<Token?> LoginAsync(LoginRequestDTO request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null; 
            }

            var accessToken = GenerateAccessToken(user);

            //update user 

            user.LastLoginAt = DateTime.UtcNow;
            _context.SaveChanges();

            return new Token
            {
                AccessToken = accessToken
            };
        }

        private string GenerateAccessToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["AccessTokenExpirationMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }
}
