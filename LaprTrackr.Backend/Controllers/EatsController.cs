using LaprTrackr.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LaprTrackr.Backend.Controllers
{
  [Authorize]
  [ApiController]
  [Route("eats")]
  public class EatsController : ControllerBase
  {
        [HttpGet("{id}")]
        public async Task<ActionResult<Eat>> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<Eat>> Add(Eat model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<Eat>> Update(Eat model)
        {
            throw new NotImplementedException();
        }
  }
}
