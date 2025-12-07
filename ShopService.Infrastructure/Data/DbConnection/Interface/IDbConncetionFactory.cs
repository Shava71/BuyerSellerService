using System.Data;

namespace ShopService.Infrastructure.Data.DbConnection;

public interface IDbConncetionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default);
}