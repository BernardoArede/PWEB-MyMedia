using System.ComponentModel.DataAnnotations;
namespace MyMedia.RCL.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(6, ErrorMessage = "A password deve ter pelo menos 6 caracteres")]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password", ErrorMessage = "As passwords não coincidem")]
        public string ConfirmPassword { get; set; } = "";

        public string? NIF { get; set; }
    }
}
