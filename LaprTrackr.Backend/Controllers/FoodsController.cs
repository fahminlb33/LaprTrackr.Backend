using LaprTrackr.Backend.Models;
using LaprTrackr.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<ActionResult<Food>> Get(int id)
        {
            Food result = await _foodService.GetById(id);
            return result;
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Food>> FindByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<Food>> Add(Food model)
        {
            Food result = await _foodService.Create(model);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<Food>> Update(Food model)
        {
            throw new NotImplementedException();
        }
    }
}
