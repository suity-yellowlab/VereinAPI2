using System.Text.Json.Serialization;
namespace VereinAPI2.Models
{
    public class Mitglied
    {
        public int? ID { get; set; }
        public string? Prename { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Anrede { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Birthplace { get; set; }
        [JsonPropertyName("banking")]
        public Banking? Banking { get; set; } = new Banking();
        [JsonPropertyName("contact")]
        public Contact? Contact { get; set; } = new Contact();

        public int? Promo { get; set; }
        public bool? Mentor { get; set; }
    }
}
