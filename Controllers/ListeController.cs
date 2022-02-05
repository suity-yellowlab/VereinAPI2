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
    public class ListeController : ControllerBase
    {
        protected ListeAccessProvider LAP { get; }
        public AppDbAccessProvider Db { get; }
        public ListeController(AppDbAccessProvider db)
        {
            Db = db;
            LAP = new ListeAccessProvider(db);
        }
        [HttpGet]
        public async Task<IEnumerable<VereinListe>> Get()
        {
            return await LAP.LoadListenAsync();

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, VereinListe item)
        {
            if (id != item.ID)
            {
                return BadRequest();
            }
            await LAP.UpdateListeAsync(item);
            return NoContent();
        }
        [HttpPost]
        public async Task<VereinListe> Post(VereinListe item)
        {
            await LAP.InsertListeAsync(item);
            return item;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await LAP.DeleteListeAsync(id);
            return NoContent();

        }
    }
}
