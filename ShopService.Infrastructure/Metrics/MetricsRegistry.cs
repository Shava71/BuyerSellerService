using Prometheus;

namespace ShopService.Infrastructure.Metrics;

public class MetricsRegistry
{
    public static readonly Counter ItemsCreated = 
        Prometheus.Metrics.CreateCounter("shop_items_created_total", "Total number of created items");

    public static readonly Counter PurchasesTotal =
        Prometheus.Metrics.CreateCounter("shop_purchases_total", "Total number of purchases");

    public static readonly Histogram PurchaseDuration =
        Prometheus.Metrics.CreateHistogram(
            "shop_purchase_duration_seconds",
            "Time of purchase",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10)
            });
}