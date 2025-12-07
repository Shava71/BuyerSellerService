using ShopService.Domain.ValueObject;

namespace ShopService.Application.Dtos;

public record UpdateItemDto(
    Guid Id,
    string Name,
    Category Category,
    decimal Price
    );