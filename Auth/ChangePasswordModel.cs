using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Auth
{
    public class ChangePasswordModel
    {
        [Required]
      
        public string? OldPassword { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 6,ErrorMessage = "Password must be 6 Characters long")]
        public string? NewPassword { get; set; }
    }
}
