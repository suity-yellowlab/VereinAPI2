using VereinAPI2.Db;
using VereinAPI2.Helpers;
using VereinAPI2.Services;
var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    var env = builder.Environment;
    services.AddCors();
    services.AddControllers();
    // services.AddEndpointsApiExplorer();
    services.Configure<DBSettings>(builder.Configuration.GetSection("DBSettings"));
    services.AddSingleton<AppDbAccessProvider>();
    //services.AddSingleton<AppDbAccessProvider>(_ => new AppDbAccessProvider(builder.Configuration["ConnectionStrings:DefaultConnection"]));
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    services.AddScoped<IUserService, UserService>();
    services.AddSingleton<IEmailService, EmailService>();

}


var app = builder.Build();




app.UseCors(
    options => options.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

// Configure the HTTP request pipeline.

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
