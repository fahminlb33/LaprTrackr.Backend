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
            return await _userService.GetById(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var user = await _userService.GetById(id);
            if (user is null)
            {
                return NotFound();
            }

            await _userService.Delete(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update([FromRoute] int id, [FromBody] User model)
        {
            var user = await _userService.GetById(id);
            if (user is null)
            {
                return NotFound();
            }

            return await _userService.Update(model);
        }
    }
}
