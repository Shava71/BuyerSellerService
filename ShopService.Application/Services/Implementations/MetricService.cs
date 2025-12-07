using Prometheus;
using ShopService.Application.Services.Interfaces;
using ShopService.Infrastructure.Metrics;

namespace ShopService.Application.Services.Implementations;

public class MetricService : IMetricService
{
    public void RecordItemPurchased()
    {
        MetricsRegistry.PurchasesTotal.Inc();
    }

    public void RecordItemCreated()
    {
        MetricsRegistry.ItemsCreated.Inc();
    }

    public IDisposable BeginPurchaseTimer()
    {
        return MetricsRegistry.PurchaseDuration.NewTimer();
    }
}