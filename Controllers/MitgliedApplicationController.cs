using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Models;

namespace VereinAPI2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MitgliedApplicationController : ControllerBase
    {
        protected MitgliedApplicationAccessProvider MAAP { get; }
        public AppDbAccessProvider Db { get; }
        public MitgliedApplicationController(AppDbAccessProvider db)
        {
            Db = db;
            MAAP = new MitgliedApplicationAccessProvider(db);
        }
        [HttpGet]
        public async Task<IEnumerable<MitgliedApplication>> Get()
        {
            return await MAAP.LoadApplicationsAsync();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await MAAP.DeleteApplicationAsync(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> AcceptApplication(int id)
        {
            await MAAP.AcceptApplicationAsync(id);
            return Ok();
        }
    }
}
