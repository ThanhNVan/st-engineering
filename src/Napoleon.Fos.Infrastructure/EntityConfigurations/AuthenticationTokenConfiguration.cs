using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Infrastructure.EntityConfigurations;

namespace Napoleon.Fos.Infrastructure.EntityConfigurations;

public class AuthenticationTokenConfiguration: BaseEntityConfiguration<AuthenticationToken>
{
    public override void Configure(EntityTypeBuilder<AuthenticationToken> builder)
    {
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ValidTill).IsRequired();
        base.Configure(builder);
    }
}
