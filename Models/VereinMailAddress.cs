using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace VereinAPI2.Models
{
    public class VereinMailAddress
    {
        public VereinMailAddress() { }
        public VereinMailAddress(MailAddress ma)
        {
            Email = ma.Address;
            DisplayName = ma.DisplayName;
        }
        public MailAddress ToMailAddress()
        {
            return new MailAddress(Email ?? string.Empty, DisplayName);
        }
        [Required]
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
    }
}
