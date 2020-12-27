using LaprTrackr.Backend.DTO;
using LaprTrackr.Backend.Infrastructure;
using LaprTrackr.Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Services
{
    public interface IAuthService
    {
        Task<AuthenticateResponseDto> Authenticate(AuthenticateDto model);

        Task<AuthenticateResponseDto> CreateEphemeralAccount();

        Task<AuthenticateResponseDto> RegisterUser(User model);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly LaprTrackrContext _context;

        public AuthService(IConfiguration config, LaprTrackrContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<AuthenticateResponseDto> Authenticate(AuthenticateDto model)
        {
            if (!_context.Users.Any(x => x.Email == model.Email))
            {
                throw new LaprTrackrException(LaprTrackrStatusCodes.NotFound, "User not found.");
            }

            var user = await _context.Users.SingleAsync(x => x.Email == model.Email);
            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = GenerateJSONWebToken(user),
                User = user
            };

            return authenticateResponseDto;
        }

        public async Task<AuthenticateResponseDto> CreateEphemeralAccount()
        {
            User user = new User()
            {
                Username = "User",
                Role = "User",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = GenerateJSONWebToken(user),
                User = user
            };

            return authenticateResponseDto;
        }

        public async Task<AuthenticateResponseDto> RegisterUser(User model)
        {
            if (_context.Users.Any(x => x.Email.ToUpperInvariant() == model.Email.ToUpperInvariant()))
            {
                throw new LaprTrackrException(LaprTrackrStatusCodes.AlreadyExists, "Email already used.");
            }

            var entityEntry = await _context.Users.AddAsync(model);
            int num = await _context.SaveChangesAsync();

            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = GenerateJSONWebToken(model),
                User = model
            };

            return authenticateResponseDto;
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
                new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Role, userInfo.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
