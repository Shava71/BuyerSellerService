using MediatR;
using Prometheus;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Domain.Events;
using ShopService.Domain.Repositories;
using ShopService.Infrastructure.Metrics;

namespace ShopService.Application.Services.Implementations;

public class PurchaseService : IPurchaseService
{
    private readonly IItemsRepository _itemsRepo;
    private readonly IMediator _mediator;
    private readonly IMetricService _metrics;

    public PurchaseService(IItemsRepository itemsRepo, IMediator mediator, IMetricService metrics)
    {
        _itemsRepo = itemsRepo;
        _mediator = mediator;
        _metrics = metrics;
    }

    public async Task PurchaseAsync(IEnumerable<Guid> itemIds, CancellationToken ct = default)
    {
        using var timer = _metrics.BeginPurchaseTimer(); // замер времени в prometheus
        
        Purchase purchase = await _itemsRepo.PurchaseItemWithLockAsync(itemIds, ct);
        List<Item> items = purchase.Items;

        foreach (Item item in items)
        {
            ItemPurchasedEvent @event = new ItemPurchasedEvent(
                ItemId: item.Id,
                PurchasedAt: purchase.PurchasedAt,
                Price: item.Price,
                Category: item.Category
                );
            
            await _mediator.Publish(@event, ct);

        }
    }
}