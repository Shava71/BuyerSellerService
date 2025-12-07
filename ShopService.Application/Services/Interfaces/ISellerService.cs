using ShopService.Application.Dtos;
using ShopService.Domain.Entities;

namespace ShopService.Application.Services.Interfaces;

public interface ISellerService
{
    Task<List<Item>> GetAllAsync(CancellationToken ct = default);
    Task<Item?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Item> CreateAsync(CreateItemDto dto, CancellationToken ct = default);
    Task UpdateAsync(UpdateItemDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}