using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Models
{
    public class VereinListe
    {
        public int? ID { get; set; }
        [Required]
        public string? Name { get; set; }
        public List<ListeMember> Members { get; set; } = new List<ListeMember>();
    }
    public class ListeMember
    {
        public int? MID { get; set; }


    }
}
