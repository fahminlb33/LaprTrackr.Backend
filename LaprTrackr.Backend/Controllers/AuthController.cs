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
        public async Task<ActionResult<AuthenticateResponseDto>> Authenticate(
          AuthenticateDto model)
        {
            try
            {
                _logger.LogDebug("Logging in {0}...", (object)model.Email);
                var authenticateResponseDto = await _authenticationService.Authenticate(model);
                return authenticateResponseDto;
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult> RefreshToken()
        {
            throw new NotImplementedException();
        }

        [HttpGet("ephemeral")]
        public async Task<ActionResult<AuthenticateResponseDto>> Ephemeral()
        {
            try
            {
                var user = await _authenticationService.CreateEphemeralAccount();
                _logger.LogDebug("Creating ephemeral account: {0}", user.User.UserId);

                return user;
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponseDto>> Register(
          User model)
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
        }

        [Authorize]
        [HttpPost("unregister")]
        public async Task<ActionResult> Unegister()
        {
            throw new NotImplementedException();
        }
    }
}
