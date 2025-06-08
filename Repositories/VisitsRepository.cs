using Cassandra;
using FavoritesService.Models;
using FavoritesService.Services;

namespace FavoritesService.Repositories;

public class VisitRepository
{
    private readonly CassandraSessionFactory _factory;

    public VisitRepository(CassandraSessionFactory factory)
    {
        _factory = factory;
    }

    public async Task AddVisitAsync(Visit visit)
    {
        var session = _factory.GetSession();
        var stmt = session.Prepare(@"
            INSERT INTO visit_history_by_user (id_user, visit_ts, id_property, city, country, property_type, transaction_type)
            VALUES (?, ?, ?, ?, ?, ?, ?)");
        await session.ExecuteAsync(stmt.Bind(
            visit.IdUser,
            visit.VisitTs,
            visit.IdProperty,
            visit.City,
            visit.Country,
            visit.PropertyType,
            visit.TransactionType
        ));
    }

    public async Task<List<Visit>> GetVisitsByUserAsync(Guid idUser)
    {
        var session = _factory.GetSession();
        var stmt = session.Prepare("SELECT * FROM visit_history_by_user WHERE id_user = ?");
        var rows = await session.ExecuteAsync(stmt.Bind(idUser));

        return rows.Select(row => new Visit
        {
            IdUser = row.GetValue<Guid>("id_user"),
            VisitTs = row.GetValue<DateTime>("visit_ts"),
            IdProperty = row.GetValue<Guid>("id_property"),
            City = row.GetValue<string>("city"),
            Country = row.GetValue<string>("country"),
            PropertyType = row.GetValue<string>("property_type"),
            TransactionType = row.GetValue<string>("transaction_type")
        }).ToList();
    }

    public async Task<List<Visit>> FilterVisitsByUserAsync(Guid idUser, DateTime? from, DateTime? to, string? propertyType, string? transactionType)
{
    var session = _factory.GetSession();

    var stmt = session.Prepare("SELECT * FROM visit_history_by_user WHERE id_user = ?");

    var rows = await session.ExecuteAsync(stmt.Bind(idUser));

    var result = rows
        .Select(row => new Visit
        {
            IdUser = row.GetValue<Guid>("id_user"),
            VisitTs = row.GetValue<DateTime>("visit_ts"),
            IdProperty = row.GetValue<Guid>("id_property"),
            City = row.GetValue<string>("city"),
            Country = row.GetValue<string>("country"),
            PropertyType = row.GetValue<string>("property_type"),
            TransactionType = row.GetValue<string>("transaction_type")
        })
        .Where(v =>
            (!from.HasValue || v.VisitTs >= from) &&
            (!to.HasValue || v.VisitTs <= to) &&
            (string.IsNullOrEmpty(propertyType) || v.PropertyType == propertyType) &&
            (string.IsNullOrEmpty(transactionType) || v.TransactionType == transactionType)
        )
        .ToList();

    return result;
}
}