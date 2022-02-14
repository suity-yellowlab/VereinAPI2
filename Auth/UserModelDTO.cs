namespace VereinAPI2.Auth
{
    public class UserModelDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
