namespace ShopService.Application.Services.Interfaces;

public interface IPurchaseService
{
    Task PurchaseAsync(IEnumerable<Guid> itemIds, CancellationToken ct = default);
}