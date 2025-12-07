using MediatR;
using ShopService.Domain.ValueObject;

namespace ShopService.Domain.Events;

public record ItemPurchasedEvent(
    Guid ItemId, 
    Category Category,
    decimal Price,
    DateTime PurchasedAt
    ) : INotification;