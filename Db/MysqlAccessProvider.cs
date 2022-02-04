using MySqlConnector;
namespace VereinAPI2.Db
{
    public static class SqlDataReaderExtensions
    {
        public static T? Get<T>(this MySqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
                return default;
            return reader.GetFieldValue<T>(ordinal);
        }

    }
    public class MysqlAccessProvider
    {
        public AppDbAccessProvider Db { get; }
        public MysqlAccessProvider(AppDbAccessProvider db)
        {
            Db = db;
        }





    }





}