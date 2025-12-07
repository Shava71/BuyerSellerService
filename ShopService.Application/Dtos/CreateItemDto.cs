using ShopService.Domain.ValueObject;

namespace ShopService.Application.Dtos;

public record CreateItemDto(
    string Name,
    Category Category,
    decimal Price
    );