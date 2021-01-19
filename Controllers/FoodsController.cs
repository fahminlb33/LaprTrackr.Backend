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
    [Route("foods")]
    public class FoodsController : ControllerBase
    {
        private readonly ILogger<FoodsController> _logger;
        private readonly IFoodService _foodService;

        public FoodsController(ILogger<FoodsController> logger, IFoodService foodService)
        {
            _logger = logger;
            _foodService = foodService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> Get([FromRoute] int id)
        {
            try
            {
                return await _foodService.GetById(id);
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

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Food>> FindByBarcode([FromRoute] string barcode)
        {
            try
            {
                return await _foodService.FindByBarcode(barcode);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to find food";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Food>> Add([FromBody] Food model)
        {
            try
            {
                return await _foodService.Create(model);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to add food";
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
                var food = await _foodService.GetById(id);
                if (food is null)
                {
                    _logger.LogDebug($"Food with ID {id} is not found");
                    return NotFound();
                }

                await _foodService.Delete(food);
                return Ok();
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to delete food";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<Food>> Update([FromRoute] int id, [FromBody] Food model)
        {
            try
            {
                var food = await _foodService.GetById(id);
                if (food is null)
                {
                    _logger.LogDebug($"Food with ID {id} is not found");
                    return NotFound();
                }

                return await _foodService.Update(model);
            }
            catch (LaprTrackrException ex)
            {
                _logger.LogDebug(ex.Message);
                return ex.GetActionResult();
            }
            catch (Exception ex)
            {
                const string message = "Failed to update food";
                _logger.LogError(ex, message);
                return new LaprTrackrException(LaprTrackrStatusCodes.ServiceUnavailable, message).GetActionResult();
            }
        }
    }
}
