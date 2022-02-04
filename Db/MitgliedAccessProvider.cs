using VereinAPI2.Models;
namespace VereinAPI2.Db
{
    public class MitgliedAccessProvider : MysqlAccessProvider
    {
        public MitgliedAccessProvider(AppDbAccessProvider db) : base(db)
        {
        }
        public async Task<IEnumerable<Mitglied>> GetMitgliederAsync()
        {
            using var conn = Db.CreateConnection();
            var mitglieder = new List<Mitglied>();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT m.ID,m.Name,m.Prename,m.Anrede,m.Titel,c.Street,c.ZIP,c.City,c.Country,c.Email,b.IBAN,b.BIC,b.MandateReference,b.CreditorID,b.Due,b.MandateDate,m.Birthdate,m.Birthplace,m.Promo,m.Mentor FROM Mitglieder m LEFT JOIN Contact c ON m.ID = c.MID LEFT JOIN Banking b ON m.ID = b.MID";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var m = new Mitglied
                {
                    ID = reader.Get<int>("ID"),
                    Name = reader.Get<string?>("Name"),
                    Prename = reader.Get<string?>("Prename"),
                    Anrede = reader.Get<string?>("Anrede"),
                    Title = reader.Get<string?>("Titel"),
                    Contact = new Contact
                    {
                        Street = reader.Get<string?>("Street"),
                        ZIP = reader.Get<string?>("ZIP"),
                        City = reader.Get<string?>("City"),
                        Country = reader.Get<string?>("Country"),
                        Email = reader.Get<string?>("Email"),
                    },
                    Banking = new Banking
                    {
                        IBAN = reader.Get<string?>("IBAN"),
                        BIC = reader.Get<string?>("BIC"),
                        Mandate = reader.Get<string?>("MandateReference"),
                        CredID = reader.Get<string?>("CreditorID"),
                        Due = reader.Get<int?>("Due"),
                        MandateDate = reader.Get<DateTime?>("MandateDate"),
                    },
                    Birthdate = reader.Get<System.DateTime?>("Birthdate"),
                    Birthplace = reader.Get<string?>("Birthplace"),
                    Promo = reader.Get<int?>("Promo"),
                    Mentor = reader.Get<bool?>("Mentor"),

                };
                mitglieder.Add(m);

            }
            return mitglieder;
        }
        public async Task DeleteMitgliedAsnyc(int id)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"DELETE FROM Mitglieder WHERE ID=@ID LIMIT 1";
            command.Parameters.AddWithValue("@ID", id);
            await command.ExecuteNonQueryAsync();
        }
        public async Task InsertMitgliedAsync(Mitglied m)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            using var command = conn.CreateCommand();
            command.Transaction = transaction;
            try
            {
                command.CommandText = @"INSERT INTO Mitglieder (Name,Prename,Anrede,Titel,Birthdate,Birthplace,Promo,Mentor) VALUES (@name,@prename,@anrede,@title,@birthdate,@birthplace,@promo,@mentor)";
                command.Parameters.AddWithValue("@name", m.Name);
                command.Parameters.AddWithValue("@prename", m.Prename);
                command.Parameters.AddWithValue("@anrede", m.Anrede);
                command.Parameters.AddWithValue("@title", m.Title);
                command.Parameters.AddWithValue("@birthdate", m.Birthdate);
                command.Parameters.AddWithValue("@birthplace", m.Birthplace);
                command.Parameters.AddWithValue("@promo", m.Promo);
                command.Parameters.AddWithValue("@mentor", m.Mentor);
                await command.ExecuteNonQueryAsync();
                m.ID = (int)command.LastInsertedId;
                if (m.Contact != null)
                {
                    var contact = m.Contact;
                    command.CommandText = @"INSERT INTO Contact (Street,ZIP,City,Country,Email,MID) VALUES (@street,@zip,@city,@country,@email,@id)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@street", contact.Street);
                    command.Parameters.AddWithValue("@zip", contact.ZIP);
                    command.Parameters.AddWithValue("@city", contact.City);
                    command.Parameters.AddWithValue("@country", contact.Country);
                    command.Parameters.AddWithValue("@email", contact.Email);
                    command.Parameters.AddWithValue("@id", m.ID);
                    await command.ExecuteNonQueryAsync();
                }
                if (m.Banking != null)
                {
                    var banking = m.Banking;
                    command.CommandText = @"INSERT INTO Banking (IBAN,BIC,MandateReference,CreditorID,Due,MID) VALUES (@iban,@bic,@mandate,@credid,@due,@id)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@iban", banking.IBAN);
                    command.Parameters.AddWithValue("@bic", banking.BIC);
                    command.Parameters.AddWithValue("@mandate", banking.Mandate);
                    command.Parameters.AddWithValue("@credid", banking.CredID);
                    command.Parameters.AddWithValue("@due", banking.Due);
                    command.Parameters.AddWithValue("@id", m.ID);
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
        public async Task SaveMitgliedAsync(Mitglied m)
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            using var command = conn.CreateCommand();
            command.Transaction = transaction;
            try
            {
                command.CommandText = @"UPDATE Mitglieder SET Name=@name,Prename=@prename,Anrede=@anrede,Titel=@title,Birthdate=@birthdate,Birthplace=@birthplace,Promo=@promo,Mentor=@mentor WHERE ID = @id LIMIT 1";
                command.Parameters.AddWithValue("@name", m.Name);
                command.Parameters.AddWithValue("@prename", m.Prename);
                command.Parameters.AddWithValue("@anrede", m.Anrede);
                command.Parameters.AddWithValue("@title", m.Title);
                command.Parameters.AddWithValue("@birthdate", m.Birthdate);
                command.Parameters.AddWithValue("@birthplace", m.Birthplace);
                command.Parameters.AddWithValue("@promo", m.Promo);
                command.Parameters.AddWithValue("@mentor", m.Mentor);
                command.Parameters.AddWithValue("@id", m.ID);
                await command.ExecuteNonQueryAsync();
                if (m.Contact != null)
                {
                    command.CommandText = @"UPDATE Contact SET Street=@street,ZIP=@zip,City=@city,Country=@country,Email=@email WHERE MID = @id LIMIT 1";
                    var contact = m.Contact;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@street", contact.Street);
                    command.Parameters.AddWithValue("@zip", contact.ZIP);
                    command.Parameters.AddWithValue("@city", contact.City);
                    command.Parameters.AddWithValue("@country", contact.Country);
                    command.Parameters.AddWithValue("@email", contact.Email);
                    command.Parameters.AddWithValue("@id", m.ID);
                    await command.ExecuteNonQueryAsync();
                }
                if (m.Banking != null)
                {
                    command.CommandText = @"UPDATE Banking SET IBAN=@iban,BIC=@bic,MandateReference=@mandate,CreditorID=@credid,Due=@due WHERE MID = @id LIMIT 1";
                    var banking = m.Banking;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@iban", banking.IBAN);
                    command.Parameters.AddWithValue("@bic", banking.BIC);
                    command.Parameters.AddWithValue("@mandate", banking.Mandate);
                    command.Parameters.AddWithValue("@credid", banking.CredID);
                    command.Parameters.AddWithValue("@due", banking.Due);
                    command.Parameters.AddWithValue("@id", m.ID);
                    await command.ExecuteNonQueryAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
