using MediatR;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Events;

namespace ShopService.Application.Handlers;

public class ItemPurchasedMetricHandler : INotificationHandler<ItemPurchasedEvent>
{
    private readonly IMetricService _metrics;

    public ItemPurchasedMetricHandler(IMetricService metrics)
    {
        _metrics = metrics;
    }

    public Task Handle(ItemPurchasedEvent notification, CancellationToken cancellationToken)
    {
        _metrics.RecordItemPurchased();
        return Task.CompletedTask;
    }
}