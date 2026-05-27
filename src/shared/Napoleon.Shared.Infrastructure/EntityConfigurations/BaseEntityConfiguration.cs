using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Napoleon.Shared.Domain.Entity;

namespace Napoleon.Shared.Infrastructure.EntityConfigurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();
            //.HasDefaultValue(DateTimeOffset.UtcNow);

            builder.Property(x => x.Id)
                .IsRequired();
            //.HasDefaultValue(Ulid.NewUlid().ToGuid());
        
        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
