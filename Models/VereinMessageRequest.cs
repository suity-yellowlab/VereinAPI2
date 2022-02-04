using System.ComponentModel.DataAnnotations;

namespace VereinAPI2.Models

{
    public class VereinMessageRequest {
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public List<VereinMessage> Messages { get; set; } = new List<VereinMessage>();
    
    }
}
