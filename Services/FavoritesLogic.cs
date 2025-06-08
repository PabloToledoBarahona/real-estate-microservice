using FavoritesService.Dtos;
using FavoritesService.Models;
using FavoritesService.Repositories;

namespace FavoritesService.Services;

public class FavoritesLogic
{
    private readonly FavoritesRepository _repo;

    public FavoritesLogic(FavoritesRepository repo)
    {
        _repo = repo;
    }

    public async Task AddFavoriteAsync(Guid userId, AddFavoriteRequest request)
    {
        var favorite = new Favorite
        {
            IdUser = userId,
            IdProperty = request.IdProperty,
            MarkedAt = DateTime.UtcNow
        };
        await _repo.AddFavoriteAsync(favorite);
    }

    public async Task<List<Favorite>> GetFavoritesAsync(Guid userId)
    {
        return await _repo.GetFavoritesByUserAsync(userId);
    }
}