using LaprTrackr.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<User>> Update(User model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("sync/{id}")]
        public async Task<ActionResult<User>> Sync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
