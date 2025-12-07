using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using ShopService.Domain.Dto;
using ShopService.Domain.Entities;
using ShopService.Domain.Repositories;
using ShopService.Domain.ValueObject;
using ShopService.Infrastructure.Data;
using ShopService.Infrastructure.Data.DbConnection;

namespace ShopService.Infrastructure.Repositories;

public class ItemsRepository : IItemsRepository
{
    private readonly ItemsDbContext _db;
    private readonly IDbConncetionFactory _connFactory;

    public ItemsRepository(ItemsDbContext db, IDbConncetionFactory connFactory)
    {
        _db = db;
        _connFactory = connFactory;
    }

    // CRUD (EF)
    public async Task<List<Item>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Item.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Item?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Item.FindAsync([id], ct).AsTask();
    }

    public async Task AddAsync(Item item, CancellationToken ct = default)
    {
        _db.Item.Add(item);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Item item, CancellationToken ct = default)
    {
        _db.Item.Update(item);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var e = await _db.Item.FindAsync(new object[]{id}, ct);
        if (e != null)
        {
            _db.Item.Remove(e);
            await _db.SaveChangesAsync(ct);
        }
    }

    // Dapper FOR UPDATE
    public async Task<Purchase> PurchaseItemWithLockAsync(IEnumerable<Guid> itemIds, CancellationToken ct = default)
    {
        using IDbConnection conn = await _connFactory.CreateOpenConnectionAsync(ct);
        
        using IDbTransaction tx = conn.BeginTransaction();

        // select for update
        const string sqlSelect = """
            SELECT * FROM "Item" WHERE "Id" = ANY(@Ids) FOR UPDATE
            """;

        List<Item> items = (await conn.QueryAsync<Item>(sqlSelect, new { Ids = itemIds.ToArray() }, tx))
            .ToList();

        if (items.Count != itemIds.Count())
        {
            throw new InvalidOperationException("Some items not found");
        }

        if (items.Any(i => i.IsSold))
        {
            throw new InvalidOperationException("One of items already sold");
        }

        Guid purchaseId = Guid.NewGuid();

        const string sqlInsertPurchase = """
                                         INSERT INTO "Purchase" ("Id", "PurchasedAt") VALUES (@Id, @PurchasedAt);
                                         """; // Создание покупки

        DateTime purchasedAt = DateTime.UtcNow;
        await conn.ExecuteAsync(sqlInsertPurchase, 
            new { Id = purchaseId, PurchasedAt = purchasedAt }, tx);

        // Помечаем items как проданные
        const string sqlUpdateItem = """
                                     UPDATE "Item" SET "IsSold" = TRUE, "PurchaseId" = @PurchaseId WHERE "Id" = @Id;
                                     """;

        foreach (Item item in items)
        {
            await conn.ExecuteAsync(sqlUpdateItem,
                new { Id = item.Id, PurchaseId = purchaseId }, tx);

            item.MarkSold(purchaseId);
        }

        tx.Commit();

        return new Purchase(purchaseId, purchasedAt, items);
    }
    
    public async Task<List<ItemByCategoryGroupDto>> GetSoldItemsAsync(CancellationToken ct = default)
    {
        return await _db.Item
            .AsNoTracking()
            .Where(i => i.IsSold)
            .Select(i => new ItemByCategoryGroupDto(i.Category, i.Price))
            .ToListAsync(ct);
    }
}