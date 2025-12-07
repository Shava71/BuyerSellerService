using MediatR;
using ShopService.Domain.ValueObject;

namespace ShopService.Domain.Events;

public record ItemAddedEvent(
    Guid ItemId, 
    Category Category, 
    decimal Price, 
    DateTime AddedAt
    ) : INotification;