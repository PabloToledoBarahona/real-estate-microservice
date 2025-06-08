using Cassandra;
using FavoritesService.Models;
using FavoritesService.Services;

namespace FavoritesService.Repositories;

public class FavoritesRepository
{
    private readonly CassandraSessionFactory _sessionFactory;

    public FavoritesRepository(CassandraSessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task AddFavoriteAsync(Favorite favorite)
    {
        var session = _sessionFactory.GetSession();
        var stmt = session.Prepare(@"
            INSERT INTO favorites_by_user (id_user, id_property, marked_at)
            VALUES (?, ?, ?)
        ");
        var boundStmt = stmt.Bind(favorite.IdUser, favorite.IdProperty, favorite.MarkedAt);
        await session.ExecuteAsync(boundStmt).ConfigureAwait(false);
    }

    public async Task<List<Favorite>> GetFavoritesByUserAsync(Guid userId)
    {
        var session = _sessionFactory.GetSession();
        var stmt = session.Prepare("SELECT id_user, id_property, marked_at FROM favorites_by_user WHERE id_user = ?");
        var boundStmt = stmt.Bind(userId);

        var rows = await session.ExecuteAsync(boundStmt).ConfigureAwait(false);

        var result = new List<Favorite>();

        foreach (var row in rows)
        {
            try
            {
                result.Add(new Favorite
                {
                    IdUser = row.GetValue<Guid>("id_user"),
                    IdProperty = row.GetValue<Guid>("id_property"),
                    MarkedAt = row.GetValue<DateTime>("marked_at")
                });
            }
            catch
            {
                // Puedes loggear aquí si lo deseas
                continue;
            }
        }

        return result;
    }
}