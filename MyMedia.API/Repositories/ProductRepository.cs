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

        public async Task<Product?> GetRandomActiveProductAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefaultAsync();
        }


        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}