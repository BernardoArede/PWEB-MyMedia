using MyMedia.API.Entities;

namespace MyMedia.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);

        Task<Product?> GetRandomActiveProductAsync();

        Task<Product> AddProductAsync(Product product);
    }
}