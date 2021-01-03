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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Services
{
    public interface IAuthService
    {
        Task<AuthenticateResponseDto> Authenticate(AuthenticateDto model);
        Task<AuthenticateResponseDto> RefreshToken(RefreshTokenDto model);
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
            var (token, refreshToken) = GenerateJSONWebToken(user);
            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = token,
                RefreshToken = refreshToken,
                User = user
            };

            await SaveRefreshToken(user, token, refreshToken);
            return authenticateResponseDto;
        }

        public async Task<AuthenticateResponseDto> RefreshToken(RefreshTokenDto model)
        {
            var refreshTokenEntity = await _context.RefreshTokens.Where(x => x.Email == model.Email).Include(x => x.User).SingleOrDefaultAsync();
            if (refreshTokenEntity == null)
            {
                throw new LaprTrackrException(LaprTrackrStatusCodes.AuthNotAuhenticated, "Refresh token is not valid.");
            }
            if ((DateTime.Now - refreshTokenEntity.CreatedAt).TotalDays > 7)
            {
                throw new LaprTrackrException(LaprTrackrStatusCodes.AuthNotAuhenticated, "Refresh token is expired.");
            }

            var (token, refreshToken) = GenerateJSONWebToken(refreshTokenEntity.User);
            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = token,
                RefreshToken = refreshToken,
                User = refreshTokenEntity.User
            };

            await SaveRefreshToken(refreshTokenEntity.User, token, refreshToken);
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

            var (token, refreshToken) = GenerateJSONWebToken(user);
            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = token,
                RefreshToken = refreshToken,
                User = user
            };

            await SaveRefreshToken(user, token, refreshToken);
            return authenticateResponseDto;
        }

        public async Task<AuthenticateResponseDto> RegisterUser(User model)
        {
            if (_context.Users.Any(x => x.Email.ToUpperInvariant() == model.Email.ToUpperInvariant()))
            {
                throw new LaprTrackrException(LaprTrackrStatusCodes.AlreadyExists, "Email already used.");
            }

            var entityEntry = await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();

            var (token, refreshToken) = GenerateJSONWebToken(model);
            var authenticateResponseDto = new AuthenticateResponseDto()
            {
                Token = token,
                RefreshToken = refreshToken,
                User = model
            };

            await SaveRefreshToken(model, token, refreshToken);
            return authenticateResponseDto;
        }

        private async Task SaveRefreshToken(User model, string token, string refreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens.Where(x => x.Email == model.Email).SingleOrDefaultAsync();
            if (refreshTokenEntity != null)
            {
                refreshTokenEntity.Token = refreshToken;
            }
            else
            {
                refreshTokenEntity = new RefreshToken
                {
                    Email = model.Email,
                    Token = refreshToken,
                    CreatedAt = DateTime.Now
                };
                await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            }

            await _context.SaveChangesAsync();
        }

        private (string token, string refreshToken) GenerateJSONWebToken(User userInfo)
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
            var refreshToken = new byte[126];
            RandomNumberGenerator.Fill(refreshToken);

            return (new JwtSecurityTokenHandler().WriteToken(token), Convert.ToBase64String(refreshToken));
        }
    }
}
