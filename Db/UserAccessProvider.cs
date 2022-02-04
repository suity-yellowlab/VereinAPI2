using VereinAPI2.Entities;
using VereinAPI2.Models;
namespace VereinAPI2.Db
{
    public class UserAccessProvider : MysqlAccessProvider
    {
        public UserAccessProvider(AppDbAccessProvider db) : base(db) { }

        public async Task<IList<User>> GetUsersAsync()
        {
            var list = new List<User>();
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID,FirstName,LastName,Username FROM Users";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = reader.Get<int>("ID"),
                    FirstName = reader.Get<string>("FirstName"),
                    LastName = reader.Get<string>("LastName"),
                    Username = reader.Get<string>("Username"),
                };
                list.Add(user);

            }
            return list;
        }
        public async Task<User?> GetUserAsync(int id)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID,FirstName,LastName,Username FROM Users WHERE ID=@id LIMIT 1";
            command.Parameters.AddWithValue("@id", id);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                return new User
                {
                    Id = id,
                    FirstName = reader.Get<string>("FirstName"),
                    LastName = reader.Get<string>("LastName"),
                    Username = reader.Get<string>("Username"),

                };

            }
            return null;


        }
        public async Task<User?> GetUserAuthenticatedAsync(AuthenticateRequest ar)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID,FirstName,LastName,Username,Password FROM Users WHERE Username=@Username";
            command.Parameters.AddWithValue("@Username", ar.Username);
            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null; // Nothing Found
            var pw = reader.Get<string>("Password");
            if (!BCrypt.Net.BCrypt.Verify(ar.Password, pw)) return null;
            var user = new User
            {
                Id = reader.Get<int>("ID"),
                FirstName = reader.Get<string>("FirstName"),
                LastName = reader.Get<string>("LastName"),
                Username = reader.Get<string>("Username"),
            };
            return user;

        }
        public async Task RegisterUserAsync(User user)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"INSERT INTO Users (FirstName,LastName,Username,Password) VALUES (@firstname,@lastname,@username,@password)";
            command.Parameters.AddWithValue("@firstname", user.FirstName);
            command.Parameters.AddWithValue("@lastname", user.LastName);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(user.Password));
            await command.ExecuteNonQueryAsync();
            user.Id = (int)command.LastInsertedId;
        }

    }
}
