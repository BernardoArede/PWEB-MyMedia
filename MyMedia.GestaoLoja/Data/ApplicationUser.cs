using Microsoft.AspNetCore.Identity;

namespace MyMedia.GestaoLoja.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; } // Aqui está o campo que faltava!

        [PersonalData]
        public string? NIF { get; set; }

        [PersonalData]
        public string? Address { get; set; }

        [PersonalData]
        public string? City { get; set; }

        // Campo importante para a regra de negócio: 
       // Clientes/Fornecedores registam-se como "Pendentes" (false) até aprovação
        public bool IsActive { get; set; } = true;

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }

}
