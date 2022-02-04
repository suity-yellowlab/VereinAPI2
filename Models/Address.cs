using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Models
{
    public class Address
    {
        public int? ID { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? DisplayName { get; set; }

    }
}
