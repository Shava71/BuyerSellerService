using Microsoft.EntityFrameworkCore;
using ShopService.Domain.Entities;
using ShopService.Infrastructure.Data.Configure;

namespace ShopService.Infrastructure.Data;

public class ItemsDbContext : DbContext
{
    public DbSet<Item> Item { get; set; } = null!;
    public DbSet<Purchase> Purchase { get; set; } = null!;
    
    public ItemsDbContext(DbContextOptions<ItemsDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ItemDbConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseDbConfiguration());
    }
}