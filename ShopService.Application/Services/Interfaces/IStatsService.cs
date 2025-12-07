using ShopService.Infrastructure.Stats;

namespace ShopService.Application.Services.Interfaces;

public interface IStatsService
{
    Task RefreshStatsAsync(CancellationToken ct = default);
    Task<StatsConfig> GetStatsAsync(CancellationToken ct = default);
}