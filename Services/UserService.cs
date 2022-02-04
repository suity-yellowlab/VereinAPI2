using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VereinAPI2.Db;
using VereinAPI2.Entities;
using VereinAPI2.Helpers;
using VereinAPI2.Models;
namespace VereinAPI2.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<User?> Register(RegisterRequest model);
        string GenerateJwtToken(User user);
    }

    public class UserService : IUserService
    {

        public AppDbAccessProvider Db { get; }
        protected UserAccessProvider Uap { get; }
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, AppDbAccessProvider db)
        {
            _appSettings = appSettings.Value;
            Db = db;
            Uap = new UserAccessProvider(Db);
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var user = await Uap.GetUserAuthenticatedAsync(model);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        public async Task<User?> Register(RegisterRequest model)
        {
            var user = model.CreateUser();
            await Uap.RegisterUserAsync(user);
            return user;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await Uap.GetUsersAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await Uap.GetUserAsync(id);
        }

        // helper methods

        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings?.Secret ?? string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id?.ToString() ?? string.Empty) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
