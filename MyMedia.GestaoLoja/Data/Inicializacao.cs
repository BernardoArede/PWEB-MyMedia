using Microsoft.AspNetCore.Identity;
using MyMedia.GestaoLoja.Entities;

namespace MyMedia.GestaoLoja.Data
{
    public class Inicializacao
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public Inicializacao(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            // 1. Criar Roles (Perfis) - Obrigatório pelo enunciado
            string[] roles = { "Admin", "Funcionario", "Cliente", "Fornecedor" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Criar Utilizador Administrador Padrão
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@mymedia.com",
                Email = "admin@mymedia.com",
                Name = "Administrador Principal",
                EmailConfirmed = true,
                IsActive = true,
                NIF = "999999990"
            };

            if (_userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                var result = await _userManager.CreateAsync(defaultUser, "Admin123!"); // Password forte
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
            // 3. Criar Modos de Disponibilização Iniciais [cite: 449]
            if (!_context.AvailabilityModes.Any())
            {
                _context.AvailabilityModes.AddRange(
                    new AvailabilityMode { Name = "Venda Imediata" },
                    new AvailabilityMode { Name = "Pré-Venda" },
                    new AvailabilityMode { Name = "Apenas Listagem" }
                );
                await _context.SaveChangesAsync();
            }

            // 4. Criar Categorias Iniciais (Opcional, mas ajuda nos testes)
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category { Name = "Filmes" },
                    new Category { Name = "Música" },
                    new Category { Name = "Acessórios" }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}
