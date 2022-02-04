namespace VereinAPI2.Models
{
    public class MitgliedApplication
    {
        public int? ID { get; set; }
        public string? Prename { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Anrede { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Birthplace { get; set; }
        public int? Promo { get; set; }
        public bool? Mentor { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? ZIP { get; set; }
        public string? Country { get; set; }
        public string? IBAN { get; set; }
        public string? BIC { get; set; }

        public int? Due { get; set; }
    }
}
