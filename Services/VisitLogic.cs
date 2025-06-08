using FavoritesService.Dtos;
using FavoritesService.Models;
using FavoritesService.Repositories;

namespace FavoritesService.Services;

public class VisitLogic
{
    private readonly VisitRepository _repo;

    public VisitLogic(VisitRepository repo)
    {
        _repo = repo;
    }

    public async Task AddVisitAsync(Guid userId, AddVisitRequest request)
    {
        var visit = new Visit
        {
            IdUser = userId,
            VisitTs = DateTime.UtcNow,
            IdProperty = request.IdProperty,
            City = request.City,
            Country = request.Country,
            PropertyType = request.PropertyType,
            TransactionType = request.TransactionType
        };

        await _repo.AddVisitAsync(visit);
    }

    public async Task<List<Visit>> GetVisitsAsync(Guid userId)
    {
        return await _repo.GetVisitsByUserAsync(userId);
    }

    public async Task<List<Visit>> FilterVisitsAsync(Guid userId, DateTime? from, DateTime? to, string? propertyType, string? transactionType)
    {
        return await _repo.FilterVisitsByUserAsync(userId, from, to, propertyType, transactionType);
    }
}