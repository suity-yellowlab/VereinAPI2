namespace VereinAPI2.Helpers
{
    public static class SqliteSettings
    {
        public const string Directory = "./IDFiles/";
        public const string ConnectionString = $"Data Source={Directory}Identity.db";
    }
}
