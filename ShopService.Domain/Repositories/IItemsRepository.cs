using System.Data;
using ShopService.Domain.Dto;
using ShopService.Domain.Entities;
using ShopService.Domain.ValueObject;

namespace ShopService.Domain.Repositories;

public interface IItemsRepository
{
    Task<List<Item>> GetAllAsync(CancellationToken ct = default);
    Task<Item?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Item item, CancellationToken ct = default);
    Task UpdateAsync(Item item, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);

    Task<Purchase> PurchaseItemWithLockAsync(IEnumerable<Guid> itemIds, CancellationToken ct = default); // For update для thread-safety
    Task<List<ItemByCategoryGroupDto>> GetSoldItemsAsync(CancellationToken ct = default);
}