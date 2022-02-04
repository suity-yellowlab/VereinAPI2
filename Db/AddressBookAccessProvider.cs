using System.Text;
using VereinAPI2.Models;
namespace VereinAPI2.Db
{
    public class AddressBookAccessProvider : MysqlAccessProvider
    {
        public AddressBookAccessProvider(AppDbAccessProvider db) : base(db)
        {
        }
        public async Task<IEnumerable<AddressBook>> GetAddressBooksAsync()
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID, Name FROM AddressBooks";
            var abs = new List<AddressBook>();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var addressBook = new AddressBook()
                    {
                        ID = reader.Get<int>("ID"),
                        Name = reader.Get<string>("Name"),
                    };
                    abs.Add(addressBook);

                }

            }
            command.CommandText = @"SELECT BookID,AddressID FROM AddressBookMembers";
            using (var reader = await command.ExecuteReaderAsync())
            {

                while (await reader.ReadAsync())
                {
                    var bid = reader.Get<int>("BookID");
                    var ab = abs.FirstOrDefault(a => a.ID == bid);
                    if (ab == null) continue;
                    var aid = reader.Get<int>("AddressID");
                    ab.Addresses.Add(aid);


                }
                return abs;
            }
        }
        public async Task InsertAddressBookAsnyc(AddressBook book)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            using var command = conn.CreateCommand();
            command.Transaction = transaction;
            try
            {


                command.Parameters.Clear();
                command.CommandText = @"INSERT INTO AddressBooks (Name) VALUES (@Name)";
                command.Parameters.AddWithValue("@Name", book.Name);
                await command.ExecuteNonQueryAsync();
                book.ID = (int)command.LastInsertedId;
                if (book.Addresses == null || book.Addresses.Count == 0)
                {
                    await transaction.CommitAsync();
                    return;
                }
                command.Parameters.Clear();
                const string header = @"INSERT INTO AddressBookMembers (BookID,AddressID) VALUES ";
                var strb = new StringBuilder();
                int counter = 0;

                foreach (var address in book.Addresses)
                {
                    if (counter > 0) strb.Append(",");
                    strb.Append($"(@BID{counter},@AID{counter})");
                    command.Parameters.AddWithValue($"@BID{counter}", book.ID);
                    command.Parameters.AddWithValue($"@AID{counter}", address);
                    counter++;
                }

                command.CommandText = $"{header}{strb}";
                await command.ExecuteNonQueryAsync();



                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
        public async Task DeleteAddressBookAsync(int id)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"DELETE FROM AddressBooks WHERE ID=@ID LIMIT 1";
            command.Parameters.AddWithValue("@ID", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAddressBookAsync(AddressBook book)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            using var command = conn.CreateCommand();
            command.Transaction = transaction;
            try
            {
                command.CommandText = @"UPDATE AddressBooks SET Name=@Name WHERE ID=@ID LIMIT 1";
                command.Parameters.AddWithValue("@Name", book.Name);
                command.Parameters.AddWithValue("@ID", book.ID);
                await command.ExecuteNonQueryAsync();
                command.Parameters.Clear();
                command.CommandText = @"DELETE FROM AddressBookMembers WHERE BookID=@BID";
                command.Parameters.AddWithValue("@BID", book.ID);
                await command.ExecuteNonQueryAsync();
                command.CommandText = @"INSERT INTO AddressBookMembers (BookID,AddressID) VALUES (@BID,@AID)";
                command.Parameters.Clear();
                await command.PrepareAsync();
                foreach (var aid in book.Addresses)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@BID", book.ID);
                    command.Parameters.AddWithValue("@AID", aid);
                    await command.ExecuteNonQueryAsync();

                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

    }
}
