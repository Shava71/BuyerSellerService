using MediatR;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Events;

namespace ShopService.Application.Handlers;

public class ItemAddedMetricHandler : INotificationHandler<ItemAddedEvent>
{
    private readonly IMetricService _metrics;

    public ItemAddedMetricHandler(IMetricService metrics)
    {
        _metrics = metrics;
    }

    public Task Handle(ItemAddedEvent notification, CancellationToken cancellationToken)
    {
        _metrics.RecordItemCreated();
        return Task.CompletedTask;
    }
}