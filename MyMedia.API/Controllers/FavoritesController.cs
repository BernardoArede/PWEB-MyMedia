using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMedia.API.Repositories.Interfaces;
using System.Security.Claims;

namespace MyMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] 
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoritesController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        [HttpGet("ping")] // Acede via: api/Favorites/ping
        public IActionResult Ping()
        {
            return Ok("Estou vivo!");
        }

        [HttpGet("debug-claims")]
        public IActionResult DebugClaims()
        {
            // Vamos listar tudo o que a API consegue ler do teu token
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                UserName = User.Identity?.Name,
                UserIdLido = userId,
                TodasAsClaims = claims
            });
        }

        // GET: api/Favorites
        [HttpGet]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var favorites = await _favoriteRepository.GetFavoritesAsync(userId);
            return Ok(favorites.Select(f => f.Product));
        }

        // POST: api/Favorites/5
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddFavorite(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _favoriteRepository.AddFavoriteAsync(userId, productId);
            return Ok(new { message = "Adicionado aos favoritos" });
        }

        // DELETE: api/Favorites/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _favoriteRepository.RemoveFavoriteAsync(userId, productId);
            return Ok(new { message = "Removido dos favoritos" });
        }

        // GET: api/Favorites/check/5
        [HttpGet("check/{productId}")]
        public async Task<ActionResult<bool>> CheckFavorite(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return false;

            return await _favoriteRepository.IsFavoriteAsync(userId, productId);
        }
    }
}
