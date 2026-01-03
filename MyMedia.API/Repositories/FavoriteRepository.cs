using Microsoft.EntityFrameworkCore;
using MyMedia.API.Data;
using MyMedia.API.Entities;
using MyMedia.API.Repositories.Interfaces;

namespace MyMedia.API.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product) 
                .ToListAsync();
        }

        public async Task AddFavoriteAsync(string userId, int productId)
        {
            if (!await IsFavoriteAsync(userId, productId))
            {
                var favorite = new Favorite { UserId = userId, ProductId = productId };
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(string userId, int productId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavoriteAsync(string userId, int productId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }
}
