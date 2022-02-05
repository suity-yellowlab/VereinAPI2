namespace VereinAPI2.Auth
{
    public static class DbInitializer
    {
        public static void Initalize(ApplicationDbContext context) {
        context.Database.EnsureCreated();

        }
    }
}
