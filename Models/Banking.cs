using System.Text.Json.Serialization;
namespace VereinAPI2.Models
{
    public class Banking
    {
        public string? IBAN { get; set; }
        public string? BIC { get; set; }
        [JsonPropertyName("MandateReference")]
        public string? Mandate { get; set; }

        public DateTime? MandateDate { get; set; }
        [JsonPropertyName("CreditorID")]
        public string? CredID { get; set; }

        public int? Due { get; set; }
    }
}
