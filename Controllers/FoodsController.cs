using LaprTrackr.Backend.Models;
using LaprTrackr.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Controllers
{
    [Authorize]
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
            return await _foodService.GetById(id);
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Food>> FindByBarcode([FromRoute] string barcode)
        {
            return await _foodService.FindByBarcode(barcode);
        }

        [HttpPost]
        public async Task<ActionResult<Food>> Add([FromBody] Food model)
        {
            return await _foodService.Create(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var food = await _foodService.GetById(id);
            if (food is null)
            {
                return NotFound();
            }

            await _foodService.Delete(food);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Food>> Update([FromRoute] int id, [FromBody] Food model)
        {
            var food = await _foodService.GetById(id);
            if (food is null)
            {
                return NotFound();
            }

            return await _foodService.Update(model);
        }
    }
}
