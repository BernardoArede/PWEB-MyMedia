using Microsoft.EntityFrameworkCore;
using MyMedia.API.Data;
using MyMedia.API.Entities;
using MyMedia.API.Repositories.Interfaces;

namespace MyMedia.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Regra: Apenas produtos ativos e com as relações carregadas
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.AvailabilityMode)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.AvailabilityMode)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }
    }
}