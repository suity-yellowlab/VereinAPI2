using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Models
{
    public class AttachmentEntry
    {
        [Required]
        public string? UUID { get; set; }
        [Required]
        public string? Filename { get; set; }


    }
}
