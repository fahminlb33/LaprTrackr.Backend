using LaprTrackr.Backend.Models;
using LaprTrackr.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Controllers
{
    [Authorize]
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
            return await _eatsService.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<Eat>> Add([FromRoute] Eat model)
        {
            return await _eatsService.Create(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var eat = await _eatsService.GetById(id);
            if (eat is null)
            {
                return NotFound();
            }

            await _eatsService.Delete(eat);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Eat>> Update([FromRoute] int id, [FromBody] Eat model)
        {
            var eat = await _eatsService.GetById(id);
            if (eat is null)
            {
                return NotFound();
            }

            return await _eatsService.Update(model);
        }
    }
}
