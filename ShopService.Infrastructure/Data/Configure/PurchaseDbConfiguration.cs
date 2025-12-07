using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopService.Domain.Entities;
using ShopService.Domain.Events;

namespace ShopService.Infrastructure.Data.Configure;

public class PurchaseDbConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Ignore(x => x.Price);
        builder.Property(x => x.PurchasedAt).IsRequired();
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Purchase)
            .HasForeignKey(x => x.PurchaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}