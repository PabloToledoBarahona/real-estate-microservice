using FavoritesService.Dtos;
using FavoritesService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FavoritesService.Controllers;

[ApiController]
[Route("favorites")]
public class FavoritesController : ControllerBase
{
    private readonly FavoritesLogic _logic;

    public FavoritesController(FavoritesLogic logic)
    {
        _logic = logic;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        await _logic.AddFavoriteAsync(userId, request);
        return Ok(new { message = "Propiedad marcada como favorita" });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFavorites()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        var result = await _logic.GetFavoritesAsync(userId);
        return Ok(result);
    }

    [HttpDelete("{propertyId}")]
    [Authorize]
    public async Task<IActionResult> RemoveFavorite([FromRoute] Guid propertyId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "No se pudo identificar al usuario autenticado." });

        await _logic.RemoveFavoriteAsync(userId, propertyId);
        return Ok(new { message = "Propiedad eliminada de favoritos" });
    }

}