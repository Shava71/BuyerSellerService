using MediatR;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Domain.Events;
using ShopService.Domain.Repositories;

namespace ShopService.Application.Services.Implementations;

public class PurchaseService : IPurchaseService
{
    private readonly IItemsRepository _itemsRepo;
    private readonly IMediator _mediator;

    public PurchaseService(IItemsRepository itemsRepo, IMediator mediator)
    {
        _itemsRepo = itemsRepo;
        _mediator = mediator;
    }

    public async Task PurchaseAsync(IEnumerable<Guid> itemIds, CancellationToken ct = default)
    {
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