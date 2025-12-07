using ShopService.Domain.ValueObject;

namespace ShopService.Domain.Dto;

public record ItemByCategoryGroupDto(Category Category, decimal Price);