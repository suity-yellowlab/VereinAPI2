using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Models
{
    public class Promo
    {
        public int? ID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public int Sort { get; set; }
    }
}
