using Microsoft.AspNetCore.Mvc;
using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Models;
namespace VereinAPI2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MailAccountController : ControllerBase
    {
        protected MailAccountProvider MAP { get; }
        public AppDbAccessProvider Db { get; }
        public MailAccountController(AppDbAccessProvider db)
        {
            Db = db;
            MAP = new MailAccountProvider(db);
        }


        [HttpGet]
        public async Task<IEnumerable<MailAccount>> Get()
        {
            return await MAP.GetMailAccountsAsync();
        }
    }
}
