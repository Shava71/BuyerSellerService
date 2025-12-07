using MediatR;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Events;

namespace ShopService.Application.Handlers;

public class ItemPurchasedHandler : INotificationHandler<ItemPurchasedEvent>
{
    private readonly IStatsService _stats;

    public ItemPurchasedHandler(IStatsService stats)
    {
        _stats = stats;
    }

    public async Task Handle(ItemPurchasedEvent notification, CancellationToken cancellationToken)
    {
        await _stats.RefreshStatsAsync(cancellationToken);
    }
}