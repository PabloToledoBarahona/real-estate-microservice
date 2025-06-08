namespace FavoritesService.Models;

public class Favorite
{
    public Guid IdUser { get; set; }
    public Guid IdProperty { get; set; }
    public DateTime MarkedAt { get; set; }
}