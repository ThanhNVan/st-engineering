using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Infrastructure.EntityConfigurations;

namespace Napoleon.Fos.Infrastructure.EntityConfigurations;

public class ProductDetailConfiguration : BaseEntityConfiguration<ProductDetail>
{
    public override void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder.Property(x => x.Varience).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.Description).HasMaxLength(1024);
        builder.Property(x => x.ProductId).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Price).IsRequired().HasMaxLength(36);
        base.Configure(builder);
    }
}
