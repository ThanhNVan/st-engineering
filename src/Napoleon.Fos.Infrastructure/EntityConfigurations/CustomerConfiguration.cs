using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Infrastructure.EntityConfigurations;

namespace Napoleon.Fos.Infrastructure.EntityConfigurations;

public class CustomerConfiguration : BaseEntityConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Age);
        builder.Property(x => x.Name).HasMaxLength(128);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Password).IsRequired();
        base.Configure(builder);
    }
}
