using System.ComponentModel.DataAnnotations;

namespace MyMedia.API.DTOs
{
    public class RegisterDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? NIF { get; set; }
    }
}
