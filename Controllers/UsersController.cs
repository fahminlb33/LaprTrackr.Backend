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
    [Authorize(Roles = UserRoles.UserAndAdmin)]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<FoodsController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<FoodsController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get([FromRoute] int id)
        {
            try
            {
                return await _userService.GetById(id);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to get food";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var user = await _userService.GetById(id);
                if (user is null)
                {
                    _logger.LogDebug($"User with ID {id} is not found");
                    return NotFound();
                }

                await _userService.Delete(user);
                return Ok();
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to get food";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update([FromRoute] int id, [FromBody] User model)
        {
            try
            {
                var user = await _userService.GetById(id);
                if (user is null)
                {
                    _logger.LogDebug($"User with ID {id} is not found");
                    return NotFound();
                }

                return await _userService.Update(model);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to get food";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }
    }
}
