using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VereinAPI2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        protected AddressAccessProvider AAP { get; }
        public AppDbAccessProvider Db { get; }
        public AddressController(AppDbAccessProvider db)
        {
            Db = db;
            AAP = new AddressAccessProvider(db);

        }
        [HttpGet]
        public async Task<IEnumerable<Address>> Get()
        {
            return await AAP.GetAddressesAsync();
        }

        // POST api/<AddressController>
        [HttpPost]
        public async Task<Address> Post(Address address)
        {
            await AAP.InsertAddressAsync(address);
            return address;
        }

        // PUT api/<AddressController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Address address)
        {
            if (id != address.ID)
            {
                return BadRequest();
            }
            await AAP.UpdateAddressAsync(address);
            return NoContent();
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await AAP.DeleteAddressAsync(id);
            return NoContent();


        }
    }
}
