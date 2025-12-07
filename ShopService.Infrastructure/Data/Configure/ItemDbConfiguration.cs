using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopService.Domain.Entities;

namespace ShopService.Infrastructure.Data.Configure;

public  class ItemDbConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Category).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.IsSold)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.PurchaseId)
            .IsRequired(false);
    }
}