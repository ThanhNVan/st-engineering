using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Infrastructure.EntityConfigurations;

namespace Napoleon.Fos.Infrastructure.EntityConfigurations;

public class CartConfiguration : BaseEntityConfiguration<Cart>
{
    public override void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ProductDetailId).IsRequired();
        builder.Property(x => x.Quantity);
        base.Configure(builder);
    }
}
