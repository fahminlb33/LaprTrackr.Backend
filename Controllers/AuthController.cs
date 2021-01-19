using LaprTrackr.Backend.DTO;
using LaprTrackr.Backend.Infrastructure;
using LaprTrackr.Backend.Models;
using LaprTrackr.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authenticationService;

        public AuthController(ILogger<AuthController> logger, IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponseDto>> Authenticate(AuthenticateDto model)
        {
            try
            {
                _logger.LogDebug("Logging in {0}...", model.Email);
                var authenticateResponseDto = await _authenticationService.Authenticate(model);
                return authenticateResponseDto;
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to authenticate user";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<AuthenticateResponseDto>> RefreshToken(RefreshTokenDto model)
        {
            try
            {
                _logger.LogDebug("Checking refresh token {0}...", model.Email);
                var authenticateResponseDto = await _authenticationService.RefreshToken(model);
                return authenticateResponseDto;
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to get new token from refresh token";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponseDto>> Register(User model)
        {
            try
            {
                AuthenticateResponseDto user = await _authenticationService.RegisterUser(model);
                _logger.LogDebug("Registering new account: {0}", user.User.UserId);

                return user;
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to register new user";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }
    }
}
