using MediatR;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Events;

namespace ShopService.Application.Handlers;

public class ItemAddedHandler : INotificationHandler<ItemAddedEvent>
{
    private readonly IStatsService _stats;

    public ItemAddedHandler(IStatsService stats)
    {
        _stats = stats;
    }

    public async Task Handle(ItemAddedEvent notification, CancellationToken cancellationToken)
    {
        await _stats.RefreshStatsAsync(cancellationToken);
    }
}