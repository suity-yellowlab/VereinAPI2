using VereinAPI2.Models;

namespace VereinAPI2.Db
{
    public class PromoAccessProvider : MysqlAccessProvider
    {
        public PromoAccessProvider(AppDbAccessProvider db) : base(db)
        {
        }
        public async Task<IEnumerable<Promo>> LoadPromosAsync()
        {
            using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            using var command = conn.CreateCommand();
            command.CommandText = @"SELECT ID, Name, Type, Sort FROM Promo";
            using var reader = await command.ExecuteReaderAsync();
            var promos = new List<Promo>();
            while (await reader.ReadAsync())
            {
                var promo = new Promo()
                {
                    ID = reader.Get<int>("ID"),
                    Name = reader.Get<string>("Name"),
                    Type = reader.Get<string>("Type"),
                    Sort = reader.Get<int>("Sort"),
                };
                promos.Add(promo);
            }
            return promos;

        }
    }
}
