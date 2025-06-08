namespace FavoritesService.Models;

public class Visit
{
    public Guid IdUser { get; set; }
    public DateTime VisitTs { get; set; } = DateTime.UtcNow;
    public Guid IdProperty { get; set; }

    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
}