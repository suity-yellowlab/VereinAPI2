using VereinAPI2.Models;
namespace VereinAPI2.Db
{
    public class MailAccountProvider : MysqlAccessProvider
    {
        public MailAccountProvider(AppDbAccessProvider db) : base(db) { }

        public async Task<IEnumerable<MailAccount>> GetMailAccountsAsync()
        {
            var accounts = new List<MailAccount>();
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID,EmailAddress FROM MailAccounts";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var account = new MailAccount()
                {
                    ID = reader.Get<int>("ID"),
                    EmailAddress = reader.Get<string>("EmailAddress"),
                };
                accounts.Add(account);
            }
            return accounts;

        }
    }
}
