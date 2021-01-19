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
    [Route("eats")]
    public class EatsController : ControllerBase
    {
        private readonly ILogger<EatsController> _logger;
        private readonly IEatsService _eatsService;

        public EatsController(ILogger<EatsController> logger, IEatsService eatsService)
        {
            _logger = logger;
            _eatsService = eatsService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Eat>> Get([FromRoute] int id)
        {
            try
            {
                return await _eatsService.GetById(id);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to get eat";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Eat>> Add([FromRoute] Eat model)
        {
            try
            {
                return await _eatsService.Create(model);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to add eat";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var eat = await _eatsService.GetById(id);
                if (eat is null)
                {
                    _logger.LogDebug($"Eat with ID {id} is not found");
                    return NotFound();
                }

                await _eatsService.Delete(eat);
                return Ok();
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to delete eat";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            } 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Eat>> Update([FromRoute] int id, [FromBody] Eat model)
        {
            try
            {
                var eat = await _eatsService.GetById(id);
                if (eat is null)
                {
                    _logger.LogDebug($"Eat with ID {id} is not found");
                    return NotFound();
                }

                return await _eatsService.Update(model);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to update eat";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }
    }
}
