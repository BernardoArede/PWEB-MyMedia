using System.ComponentModel.DataAnnotations;
namespace MyMedia.RCL.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "A password é obrigatória")]
        public string Password { get; set; } = "";
    }
}
