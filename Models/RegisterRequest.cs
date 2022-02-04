using System.ComponentModel.DataAnnotations;
using VereinAPI2.Entities;
namespace VereinAPI2.Models
{
    public class RegisterRequest
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public User CreateUser()
        {
            return new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Password = Password,

            };

        }
    }
}
