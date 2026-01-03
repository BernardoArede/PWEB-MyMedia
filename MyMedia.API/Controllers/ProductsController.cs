using Microsoft.AspNetCore.Mvc;
using MyMedia.API.Entities;
using MyMedia.API.Repositories.Interfaces;

namespace MyMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Products/
        [HttpGet("highlight")]
        public async Task<ActionResult<Product>> GetHighlightProduct()
        {
            var product = await _productRepository.GetRandomActiveProductAsync();

            if (product == null)
            {
                return NoContent();
            }

            return Ok(product);
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}