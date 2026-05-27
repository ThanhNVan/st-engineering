using Microsoft.EntityFrameworkCore;
using Napoleon.Shared.Domain.Entity;
using Napoleon.Shared.Infrastructure.EntityConfigurations;

namespace Napoleon.Shared.Infrastructure.PersistenceDbContext;

public abstract class BaseDbContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplySoftDeleteQueryFilter();
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                     .Where(q => q.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.Entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                    break;

                case EntityState.Added:
                    if(entry.Entity.CreatedAt == DateTimeOffset.MinValue || entry.Entity.CreatedAt == DateTimeOffset.MaxValue) 
                        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;

                    if (entry.Entity.Id == Guid.Empty) 
                        entry.Entity.Id = Ulid.NewUlid().ToGuid();
                    break;

                case EntityState.Modified:
                    Entry(entry.Entity).Property(x => x.CreatedAt).IsModified = false;
                    break;

                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();

    #region [ GetDbSet ]
    protected static IQueryable<BaseEntity>? GetDbSet(DbContext context, Type T)
    {
        var method = typeof(DbContext).GetMethods().Single(p =>
            p is { Name: nameof(Set), ContainsGenericParameters: true } && p.GetParameters().Length <= 0);

        // Build a method with the specific type argument you're interested in
        method = method.MakeGenericMethod(T);

        return method.Invoke(context, null) as IQueryable<BaseEntity>;
    }
    #endregion
}

