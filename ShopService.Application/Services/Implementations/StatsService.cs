using Microsoft.Extensions.Options;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Dto;
using ShopService.Domain.Repositories;
using ShopService.Domain.ValueObject;
using ShopService.Infrastructure.Stats;

namespace ShopService.Application.Services.Implementations;

public class StatsService : IStatsService
{
    private readonly IItemsRepository _repo;
    private readonly IOptionsMonitorCache<StatsConfig> _cache;
    private readonly IOptionsMonitor<StatsConfig> _monitor;
    
    public StatsService(
        IItemsRepository repo,
        IOptionsMonitorCache<StatsConfig> cache,
        IOptionsMonitor<StatsConfig> monitor
        )
    {
        _repo = repo;
        _cache = cache;
        _monitor = monitor;
    }

    public async Task RefreshStatsAsync(CancellationToken ct = default) // самое худшее моё место в коде, аж стыдно за всю грязь и зависимости; todo: изучить, как можно убрать IOptions в отдельные классы
    {
        List<ItemByCategoryGroupDto> sold = await _repo.GetSoldItemsAsync(ct);

        if (sold.Count == 0)
        {
            _cache.TryRemove(Options.DefaultName);
            return;
        }

        var top = sold
            .GroupBy(s => s.Category)
            .Select(g => new 
            { 
                Category = g.Key,
                Sum = g.Sum(x => x.Price),
                AvgPrice = g.Average(x => x.Price)
            })
            .OrderByDescending(x => x.Sum)
            .First();

        // новые настройки
        var updated = new StatsConfig
        {
            TopCategory = top.Category.ToString(),
            AveragePrice = top.AvgPrice
        };
        
        _cache.TryRemove(Options.DefaultName);

        // пересоздание
        _cache.TryAdd(Options.DefaultName, updated);
    }

    public async Task<StatsConfig> GetStatsAsync(CancellationToken ct = default)
    {
        StatsConfig current = _monitor.CurrentValue;
        return await Task.FromResult(current);
    }
}