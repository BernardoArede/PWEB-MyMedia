using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMedia.API.Data;
using MyMedia.API.Entities;
using MyMedia.API.Repositories.Interfaces;
using System.Security.Claims;

namespace MyMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public ProductsController(IProductRepository productRepository, AppDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }


        // POST: api/Products
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized("Utilizador não identificado.");

            product.SupplierId = userId;
            product.IsActive = true;
            if (product.AvailabilityModeId == 0) product.AvailabilityModeId = 1;

            var settings = await _context.CompanySettings.FirstOrDefaultAsync();

           
            decimal margin = settings != null ? settings.ProfitMargin : 0.50m;

            if (product.BasePrice > 0)
            {
                product.Price = product.BasePrice + (product.BasePrice * margin);
            }
            else if (product.Price > 0)
            {
           
                product.BasePrice = product.Price;
            }
          
            try
            {
                var newProduct = await _productRepository.AddProductAsync(product);
                return CreatedAtAction("GetProduct", new { id = newProduct.Id }, newProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar produto: {ex.Message}");
                return BadRequest($"Erro ao gravar produto. Detalhes: {ex.Message}");
            }
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