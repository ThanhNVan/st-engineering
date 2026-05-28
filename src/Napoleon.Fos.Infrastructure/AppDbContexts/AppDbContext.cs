using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Fos.Infrastructure.Extensions;
using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;
using Napoleon.Shared.Infrastructure.PersistenceDbContext;

namespace Napoleon.Fos.Infrastructure.AppDbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> contextOptions) : BaseDbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyEntityConfiguration();
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = base.ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        await OnAuditTrailAsync(entries, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    #region [ DbSet ]

    public DbSet<AuditTrail> AuditTrails { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<AuthenticationToken> AuthenticationTokens { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductDetail> ProductDetails { get; set; }

    #endregion

    #region [ Audit Trails ]
    private async Task OnAuditTrailAsync(IList<EntityEntry<BaseEntity>> entries, CancellationToken cancellationToken = default)
    {
        foreach (var entry in entries)
            switch (entry.State)
            {
                case EntityState.Deleted:
                    await OnDeleteAuditAsync(entry, cancellationToken);
                    break;

                case EntityState.Modified:
                    await OnModifyAuditAsync(entry, cancellationToken);
                    break;

                case EntityState.Added:
                    await OnAddAuditAsync(entry, cancellationToken);
                    break;

                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    break;
            }
    }

    private async Task OnDeleteAuditAsync(EntityEntry<BaseEntity> entry, CancellationToken cancellationToken = default)
    {
        if (entry.Entity is IDeletingAuditTrail)
            await AuditTrails.AddAsync(entry.Entity.GetDeletingAuditTrail(), cancellationToken);
    }
    private async Task OnAddAuditAsync(EntityEntry<BaseEntity> entry, CancellationToken cancellationToken = default)
    {
        if (entry.Entity is IAddingAuditTrail)
        {
            var audit = entry.Entity.GetAddingAuditTrail();
            await AuditTrails.AddAsync(audit, cancellationToken);
        }
    }

    private async Task OnModifyAuditAsync(EntityEntry<BaseEntity> entry, CancellationToken cancellationToken = default)
    {
        if (entry.Entity is IModifyingAuditTrail)
        {
            var beforeValue = await GetDbSet(this, entry.Entity.GetType())?
                                                .FirstOrDefaultAsync(x => x.Id == entry.Entity.Id, cancellationToken)!;

            var compObjects = new CompareLogic
            {
                Config =
                {
                    MaxDifferences = 99
                }
            };

            var compResult = compObjects.Compare(beforeValue, entry.Entity);

            var deltaList = compResult.Differences
                .Where(x => x.PropertyName != "CreatedAt")
                .Select(change => new AuditDelta { FieldName = change.PropertyName[..change.PropertyName.Length], ValueBefore = change.Object1Value, ValueAfter = change.Object2Value });

            var audit = beforeValue!.GetModifyingAuditTrail(entry.Entity, deltaList);
            await Set<AuditTrail>().AddAsync(audit, cancellationToken);
        }
    }

    #endregion
}
