using Microsoft.AspNetCore.Identity;
using VereinAPI2.Auth;

namespace VereinAPI2 {

    public  class VereinConfigurator {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public static VereinConfigurator FromScope(IServiceScope scope,IConfiguration configuration) {
        var services =  scope.ServiceProvider;
        var usermanager = services.GetRequiredService<UserManager<IdentityUser>>();
        var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();
            return new VereinConfigurator(usermanager, rolemanager, configuration);
        
        }
        public VereinConfigurator(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
           
        }

        public async Task CreateDefaultAdmin() {
            var userexists = await _userManager.FindByNameAsync("Admin");
            if (userexists != null) { return; } // Already in DB
            IdentityUser user = new()
            {
                Email = "admin@example.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Admin"
            };
            var result = await _userManager.CreateAsync(user,"Start1234");
            if (!result.Succeeded) { throw new Exception($"Default Admin could not be created: {result.Errors.First().Description}"); }

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


       }
       
    }


    
}