using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Models
{
    public class AddressBook
    {
        public int? ID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public List<int> Addresses { get; set; } = new List<int>();

    }
}
