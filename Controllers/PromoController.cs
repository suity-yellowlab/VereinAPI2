using Microsoft.AspNetCore.Mvc;
using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Models;

namespace VereinAPI2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PromoController : ControllerBase
    {
        protected PromoAccessProvider PAP { get; }
        public AppDbAccessProvider Db { get; }
        public PromoController(AppDbAccessProvider db)
        {
            Db = db;
            PAP = new PromoAccessProvider(db);
        }
        [HttpGet]
        public async Task<IEnumerable<Promo>> Get()
        {
            return await PAP.LoadPromosAsync();
        }
    }
}
