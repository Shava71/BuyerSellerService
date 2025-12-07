using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ShopService.Infrastructure.Data.DbConnection.Implementation;

public class DapperConnectionFactory : IDbConncetionFactory
{
    private readonly string _connString;
    public DapperConnectionFactory(IConfiguration cfg)
    {
        _connString = cfg.GetConnectionString("Postgre");
    }
    public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default)
    {
        var cn = new NpgsqlConnection(_connString);
        await cn.OpenAsync(ct);
        return cn;
    }
}