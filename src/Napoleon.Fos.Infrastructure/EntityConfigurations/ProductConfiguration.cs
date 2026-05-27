
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Infrastructure.EntityConfigurations;

namespace Napoleon.Fos.Infrastructure.EntityConfigurations;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.ImageUrl).HasMaxLength(1024);
        builder.Property(x => x.Name).HasMaxLength(128);
        builder.Property(x => x.Description).HasMaxLength(1024);
        base.Configure(builder);
    }
}
