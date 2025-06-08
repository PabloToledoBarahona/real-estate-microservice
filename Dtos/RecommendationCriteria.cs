namespace FavoritesService.Dtos;

public class RecommendationCriteria
{
    public string City { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
}