using System.ComponentModel.DataAnnotations;
namespace MyMedia.RCL.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public string? Message { get; set; }
    }
}
