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
        await _repo.IncrementDailyVisitAsync(visit.IdProperty, visit.VisitTs);
        await _repo.IncrementZoneVisitAsync(visit.City, visit.IdProperty, visit.VisitTs);
    }

    public async Task<List<Visit>> GetVisitsAsync(Guid userId)
    {
        return await _repo.GetVisitsByUserAsync(userId);
    }

    public async Task<List<Visit>> FilterVisitsAsync(Guid userId, DateTime? from, DateTime? to, string? propertyType, string? transactionType)
    {
        return await _repo.FilterVisitsByUserAsync(userId, from, to, propertyType, transactionType);
    }

    public async Task<List<VisitDailyStat>> GetDailyVisitStats(Guid propertyId)
    {
        var rawStats = await _repo.GetDailyVisitStatsAsync(propertyId);
        return rawStats.Select(stat => new VisitDailyStat
        {
            Date = stat.Date,
            Count = stat.Count
        }).ToList();
    }

    public async Task<List<VisitZoneStat>> GetZoneVisitStats(Guid propertyId)
    {
        var rawStats = await _repo.GetZoneVisitStatsAsync(propertyId);
        return rawStats.Select(stat => new VisitZoneStat
        {
            City = stat.City,
            Date = stat.Date,
            Count = stat.Count
        }).ToList();
    }

    public async Task<RecommendationCriteria?> GetRecommendationCriteriaAsync(Guid userId)
    {
        var visits = await _repo.GetVisitsByUserAsync(userId);

        if (!visits.Any()) return null;

        var topCity = visits
            .GroupBy(v => v.City)
            .OrderByDescending(g => g.Count())
            .First().Key;

        var topType = visits
            .GroupBy(v => v.PropertyType)
            .OrderByDescending(g => g.Count())
            .First().Key;

        var topTransaction = visits
            .GroupBy(v => v.TransactionType)
            .OrderByDescending(g => g.Count())
            .First().Key;

        return new RecommendationCriteria
        {
            City = topCity,
            PropertyType = topType,
            TransactionType = topTransaction
        };
    }
}