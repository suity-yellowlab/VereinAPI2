using VereinAPI2.Models;
namespace VereinAPI2.Db
{
    public class MitgliedApplicationAccessProvider : MysqlAccessProvider
    {
        public MitgliedApplicationAccessProvider(AppDbAccessProvider db) : base(db)
        {
        }
        public async Task DeleteApplicationAsync(int id)
        {

            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"DELETE FROM Applications WHERE ID=@ID LIMIT 1";
            command.Parameters.AddWithValue("@ID", id);
            await command.ExecuteNonQueryAsync();
        }
        public async Task AcceptApplicationAsync(int id)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"UPDATE Applications SET Accepted=1 WHERE ID=@ID LIMIT 1";
            command.Parameters.AddWithValue("@ID", id);
            await command.ExecuteNonQueryAsync();
        }
        public async Task<IEnumerable<MitgliedApplication>> LoadApplicationsAsync()
        {
            var applications = new List<MitgliedApplication>();
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID,Name,Prename,Anrede,Titel,Street,ZIP,City,Country,Email,IBAN,BIC,Due,Birthdate,Birthplace,Promo,Mentor FROM Applications WHERE Accepted = 0";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {

                var a = new MitgliedApplication
                {
                    ID = reader.Get<int>("ID"),
                    Name = reader.Get<string?>("Name"),
                    Prename = reader.Get<string?>("Prename"),
                    Anrede = reader.Get<string?>("Anrede"),
                    Title = reader.Get<string?>("Titel"),

                    Street = reader.Get<string?>("Street"),
                    ZIP = reader.Get<string?>("ZIP"),
                    City = reader.Get<string?>("City"),
                    Country = reader.Get<string?>("Country"),
                    Email = reader.Get<string?>("Email"),

                    IBAN = reader.Get<string?>("IBAN"),
                    BIC = reader.Get<string?>("BIC"),

                    Due = reader.Get<int?>("Due"),
                    Promo = reader.Get<int?>("Promo"),
                    Birthdate = reader.Get<DateTime?>("Birthdate"),
                    Birthplace = reader.Get<string?>("Birthplace"),

                    Mentor = reader.Get<bool?>("Mentor"),


                };



                applications.Add(a);


            }
            return applications;
        }
    }
}