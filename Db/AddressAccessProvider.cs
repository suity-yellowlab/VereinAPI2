using VereinAPI2.Models;

namespace VereinAPI2.Db
{
    public class AddressAccessProvider : MysqlAccessProvider
    {
        public AddressAccessProvider(AppDbAccessProvider db) : base(db)
        {
        }
        public async Task<List<Address>> GetAddressesAsync()
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID, DisplayName, EmailAddress FROM Addresses";
            using (var reader = await command.ExecuteReaderAsync())
            {
                List<Address> addresses = new List<Address>();
                while (await reader.ReadAsync())
                {
                    var add = new Address()
                    {
                        ID = reader.Get<int>("ID"),
                        DisplayName = reader.Get<string>("DisplayName"),
                        Email = reader.Get<string>("EmailAddress"),
                    };
                    addresses.Add(add);
                }
                return addresses;
            }


        }
        public async Task DeleteAddressAsync(int id)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"DELETE FROM Addresses WHERE ID = @ID LIMIT 1";
            command.Parameters.AddWithValue("@ID", id);
            await command.ExecuteNonQueryAsync();

        }

        public async Task InsertAddressAsync(Address address)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"INSERT INTO Addresses (DisplayName,EmailAddress) VALUES (@DN,@EA)";
            command.Parameters.AddWithValue("@DN", address.DisplayName);
            command.Parameters.AddWithValue("@EA", address.Email);
            await command.ExecuteNonQueryAsync();
        }
        public async Task UpdateAddressAsync(Address address)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"UPDATE Addresses SET DisplayName=@DN, EmailAddress=@EA WHERE ID=@ID LIMIT 1";
            await command.PrepareAsync();
            command.Parameters.AddWithValue("@DN", address.DisplayName);
            command.Parameters.AddWithValue("@EA", address.Email);
            command.Parameters.AddWithValue("@ID", address.ID);
            await command.ExecuteNonQueryAsync();


        }


    }
}
