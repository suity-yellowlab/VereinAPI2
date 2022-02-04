
namespace VereinAPI2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using VereinAPI2.Db;
    using VereinAPI2.Helpers;
    using VereinAPI2.Models;
    using VereinAPI2.Services;

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        protected UserAccessProvider UAP { get; }
        public AppDbAccessProvider Db { get; }
        public UsersController(IUserService userService, AppDbAccessProvider db)
        {
            _userService = userService;
            Db = db;
            UAP = new UserAccessProvider(Db);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var user = await UAP.GetUserAuthenticatedAsync(model);

            // var response = await _userService.Authenticate(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(new AuthenticateResponse(user, _userService.GenerateJwtToken(user)));
        }
        /*
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request) {
            var user = request.CreateUser();
            await UAP.RegisterUserAsync(user);
   
            return Ok(user);
        }
        */


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await UAP.GetUsersAsync();
            return Ok(users);
        }
    }
}

