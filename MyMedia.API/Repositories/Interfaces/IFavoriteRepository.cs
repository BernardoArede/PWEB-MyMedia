using MyMedia.API.Entities;

namespace MyMedia.API.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId);
        Task AddFavoriteAsync(string userId, int productId);
        Task RemoveFavoriteAsync(string userId, int productId);
        Task<bool> IsFavoriteAsync(string userId, int productId);
    }
}
