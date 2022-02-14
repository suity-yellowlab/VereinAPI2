using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VereinAPI2.Auth;

namespace VereinAPI2.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPut("changepass")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword) {
            var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
            if (user == null) { throw new Exception(); }
            
           var res =  await _userManager.ChangePasswordAsync(user,changePassword.OldPassword,changePassword.NewPassword);
            if (!res.Succeeded) { throw new Exception($"PW Change failed {res.Errors.First().Description}"); }
            
            return NoContent();
        
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IList<UserModelDTO>> GetAll() {
            
            var users =  _userManager.Users.ToList();            
            if (users == null) throw new Exception("Could not query users");
            List<UserModelDTO> dtos = new();
            foreach (var user in users) {
            var roles = await _userManager.GetRolesAsync(user);
            dtos.Add(new UserModelDTO {Username = user.UserName,Email = user.Email, Roles = roles});
            
            }


            return dtos;
        
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username) { 
        var userExists = await _userManager.FindByNameAsync(username);
        if (userExists == null) { return BadRequest(); }
        var res = await _userManager.DeleteAsync(userExists);
        if (!res.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error deleting" });
        return NoContent();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

    }
}
