using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Services;
using VereinAPI2.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
{
   

    var services = builder.Services;
    var env = builder.Environment;
    var configuration = builder.Configuration;
    services.AddCors();
    services.AddControllers();
    // services.AddEndpointsApiExplorer();
  
    services.Configure<DBSettings>(builder.Configuration.GetSection("DBSettings"));
    Directory.CreateDirectory(SqliteSettings.Directory);
    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(SqliteSettings.ConnectionString));
    services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
    })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(options => { 
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters() { 
        ValidateIssuer = false,
        ValidateAudience = false,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        };
    });
    // https://stackoverflow.com/a/62642601
    services.AddAuthorization(options => {
        var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser();
        options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
    });
    services.AddSingleton<AppDbAccessProvider>();
    services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    services.AddSingleton<IEmailService, EmailService>();

}


var app = builder.Build();
using (var scope = app.Services.CreateScope()) { 
var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
       
        DbInitializer.Initalize(context);
    }
    catch (Exception ex) {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");

    }
    var configurator = VereinAPI2.VereinConfigurator.FromScope(scope,app.Configuration);
    try
    {
        await configurator.CreateDefaultAdmin();

    }
    catch (Exception ex) {
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
        throw;
    }

}





// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.UseCors( options => options.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
