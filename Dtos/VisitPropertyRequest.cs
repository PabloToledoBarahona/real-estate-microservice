using System.ComponentModel.DataAnnotations;

namespace FavoritesService.Dtos;

public class AddVisitRequest
{
    [Required]
    public Guid IdProperty { get; set; }

    [Required]
    public string PropertyType { get; set; } = string.Empty;

    [Required]
    public string TransactionType { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;
}