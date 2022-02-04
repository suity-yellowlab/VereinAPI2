using System.Text;
using VereinAPI2.Models;
namespace VereinAPI2.Db
{
    public class ListeAccessProvider : MysqlAccessProvider
    {
        public ListeAccessProvider(AppDbAccessProvider db) : base(db)
        {
        }

        public async Task<IEnumerable<VereinListe>> LoadListenAsync()
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID,Name FROM Listen";
            var lst = new List<VereinListe>();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var ls = new VereinListe
                    {
                        ID = reader.Get<int>("ID"),
                        Name = reader.Get<string>("Name"),
                    };
                    lst.Add(ls);
                }
            }
            command.CommandText = @"SELECT LID,MID FROM ListenMembers";
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {

                    var lid = reader.Get<int>("LID");
                    var mid = reader.Get<int>("MID");

                    var liste = lst.FirstOrDefault(ls => ls.ID == lid);
                    if (liste == null) continue;
                    liste.Members.Add(new ListeMember()
                    {
                        MID = mid,
                    });
                }
                return lst;


            }
        }
        public async Task UpdateListeAsync(VereinListe vl)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            using var command = conn.CreateCommand();
            command.Transaction = transaction;
            try
            {
                command.CommandText = @"UPDATE Listen SET Name=@Name WHERE ID=@ID LIMIT 1";
                command.Parameters.AddWithValue("@Name", vl.Name);
                command.Parameters.AddWithValue("@ID", vl.ID);
                await command.ExecuteNonQueryAsync();
                command.CommandText = @"DELETE FROM ListenMembers WHERE LID=@LID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@LID", vl.ID);
                await command.ExecuteNonQueryAsync();
                if (vl.Members.Count == 0) {
                await transaction.CommitAsync();
                    return;
                }
                command.Parameters.Clear();
                const string header = @"INSERT INTO ListenMembers (LID, MID) VALUES ";
                var strb = new StringBuilder();
                int counter = 0;
                foreach (var m in vl.Members)
                {
                    if (counter > 0) strb.Append(',');
                    strb.Append($"(@LID{counter},@MID{counter})");
                    command.Parameters.AddWithValue($"@LID{counter}", vl.ID);
                    command.Parameters.AddWithValue($"@MID{counter}", m.MID);
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
        public async Task InsertListeAsync(VereinListe vl)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            using var command = conn.CreateCommand();
            command.Transaction = transaction;
            try
            {
                command.CommandText = @"INSERT INTO Listen (Name) VALUES (@Name)";
                command.Parameters.AddWithValue("@Name", vl.Name);
                await command.ExecuteNonQueryAsync();
                vl.ID = (int)command.LastInsertedId;
                if (vl.Members == null || vl.Members.Count == 0)
                {
                    await transaction.CommitAsync();
                    return;
                }
                command.Parameters.Clear();
                const string header = @"INSERT INTO ListenMembers (LID, MID) VALUES ";
                var strb = new StringBuilder();
                int counter = 0;
                foreach (var m in vl.Members)
                {
                    if (counter > 0) strb.Append(',');
                    strb.Append($"(@LID{counter},@MID{counter})");
                    command.Parameters.AddWithValue($"@LID{counter}", vl.ID);
                    command.Parameters.AddWithValue($"@MID{counter}", m.MID);
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
        public async Task DeleteListeAsync(int id)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"DELETE FROM Listen WHERE ID = @ID LIMIT 1";
            command.Parameters.AddWithValue("@ID", id);
            await command.ExecuteNonQueryAsync();

        }
    }
}
