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
    public class AddressBookController : ControllerBase
    {
        protected AddressBookAccessProvider ABAP { get; }
        public AppDbAccessProvider Db { get; }
        public AddressBookController(AppDbAccessProvider db)
        {
            Db = db;
            ABAP = new AddressBookAccessProvider(db);
        }

        // GET: api/<AddressBookController>
        [HttpGet]
        public async Task<IEnumerable<AddressBook>> Get()
        {
            return await ABAP.GetAddressBooksAsync();

        }



        // POST api/<AddressBookController>
        [HttpPost]
        public async Task<AddressBook> Post(AddressBook addressBook)
        {
            await ABAP.InsertAddressBookAsnyc(addressBook);
            return addressBook;
        }

        // PUT api/<AddressBookController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AddressBook addressBook)
        {
            if (id != addressBook.ID)
            {
                return BadRequest();
            }
            await ABAP.UpdateAddressBookAsync(addressBook);
            return NoContent();
        }

        // DELETE api/<AddressBookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await ABAP.DeleteAddressBookAsync(id);
            return NoContent();
        }
    }
}
