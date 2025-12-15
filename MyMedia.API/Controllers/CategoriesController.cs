using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMedia.API.Data;
using MyMedia.API.Entities;

namespace MyMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            // Vai buscar as categorias à BD partilhada
            return await _context.Categories.ToListAsync();
        }
    }
}