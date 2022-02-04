using VereinAPI2.Helpers;
namespace VereinAPI2.Db
{
    using Microsoft.Extensions.Options;
    using MySqlConnector;
    public class AppDbAccessProvider
    {
        private string ConnectionString { get; }
        private readonly DBSettings _dBSettings;
        public AppDbAccessProvider(IOptions<DBSettings> dbsettings)
        {
            _dBSettings = dbsettings.Value;
            ConnectionString = $"server={_dBSettings.Host};user id={_dBSettings.UserID};password={_dBSettings.Password};port={_dBSettings.Port};database={_dBSettings.Database}";
        }
        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
