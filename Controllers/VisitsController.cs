using FavoritesService.Dtos;
using FavoritesService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FavoritesService.Controllers;

[ApiController]
[Route("visits")]
public class VisitController : ControllerBase
{
    private readonly VisitLogic _logic;

    public VisitController(VisitLogic logic)
    {
        _logic = logic;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddVisit([FromBody] AddVisitRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        await _logic.AddVisitAsync(userId, request);
        return Ok(new { message = "Visita registrada correctamente" });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetVisits()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        var result = await _logic.GetVisitsAsync(userId);
        return Ok(result);
    }

    [HttpGet("filter")]
    [Authorize]
    public async Task<IActionResult> FilterVisits([FromQuery] DateTime? from, [FromQuery] DateTime? to,
                                              [FromQuery] string? propertyType, [FromQuery] string? transactionType)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        var visits = await _logic.FilterVisitsAsync(userId, from, to, propertyType, transactionType);
        return Ok(visits);
    }

    [HttpGet("stats/{propertyId}")]
    [Authorize]
    public async Task<IActionResult> GetVisitStats(Guid propertyId)
    {
        var daily = await _logic.GetDailyVisitStats(propertyId);
        var zones = await _logic.GetZoneVisitStats(propertyId);

        return Ok(new { daily, zones });
    }

    [HttpGet("recommendations")]
    [Authorize]
    public async Task<IActionResult> GetRecommendations()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        var criteria = await _logic.GetRecommendationCriteriaAsync(userId);

        if (criteria is null)
            return Ok(new { message = "No hay suficientes datos para generar recomendaciones." });

        return Ok(criteria);
    }
}