using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShopService.Infrastructure.Data.Factory;

public class ItemsDbContextFactory : IDesignTimeDbContextFactory<ItemsDbContext>
{
    public ItemsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ItemsDbContext>();
        
        optionsBuilder.UseNpgsql("Host=db;Port=5432;Username=user;Password=pass;Database=db;");

        return new ItemsDbContext(optionsBuilder.Options);
    }
}