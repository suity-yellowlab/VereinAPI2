using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Models;

namespace VereinAPI2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MitgliedController : ControllerBase
    {
        protected MitgliedAccessProvider MAP { get; }
        public AppDbAccessProvider Db { get; }
        public MitgliedController(AppDbAccessProvider db)
        {
            Db = db;
            MAP = new MitgliedAccessProvider(db);
        }
        [HttpGet]
        public async Task<IEnumerable<Mitglied>> Get()
        {

            return await MAP.GetMitgliederAsync();

        }
        [HttpPost]
        public async Task<IActionResult> Post(Mitglied m)
        {
            await MAP.InsertMitgliedAsync(m);
            return Ok();

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Mitglied m)
        {
            if (id != m.ID)
            {
                return BadRequest();
            }
            await MAP.SaveMitgliedAsync(m);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await MAP.DeleteMitgliedAsnyc(id);
            return NoContent();
        }

    }
}
