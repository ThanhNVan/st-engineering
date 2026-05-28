using Microsoft.EntityFrameworkCore;
using Napoleon.Shared.Common;
using Napoleon.Shared.Common.CustomException;
using Napoleon.Shared.Core.Repositories;
using Napoleon.Shared.Domain.Entity;
using Napoleon.Shared.Infrastructure.PersistenceDbContext;
using System.Linq.Expressions;

namespace Napoleon.Shared.Infrastructure.Repositories;

public class BaseRepository<TEntity>(BaseDbContext dbContext) : IBaseRepository<TEntity>
where TEntity : BaseEntity
{
    #region [ Get ] 
    public async ValueTask<IList<TEntity>?> GetManyAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        var result = await GetQueryable()
                                    .Where(expression)
                                    .ToListAsync(cancellationToken);

        return result;
    }

    public async ValueTask<IList<TEntity>?> GetManyByIdsAsync(IList<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var result = await GetQueryable()
                            .Where(x => ids.Contains(x.Id))
                            .ToListAsync(cancellationToken);

        return result;
    }

    public async ValueTask<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        var result = await GetQueryable()
                                .Where(expression)
                                .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async ValueTask<TEntity?> GetSingleAsync(IQueryable<TEntity> expression,
        CancellationToken cancellationToken = default)
    {
        var result = await expression.FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async ValueTask<TEntity?> GetSingleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await GetQueryable()
                            .Where(x => x.Id.Equals(id))
                            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async ValueTask<TType?> GetSingleTypeAsync<TType>(IQueryable<TType> queryable,
        CancellationToken cancellationToken = default)
    {
        var result = await queryable.FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async ValueTask<IList<TType>?> GetManyTypeAsync<TType>(IQueryable<TType> queryable,
        CancellationToken cancellationToken = default)
    {
        var result = await queryable.ToListAsync(cancellationToken);

        return result;
    }
    #endregion

    #region [ Add ]

    public async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        return result.Entity;
    }

    public async ValueTask<TEntity> AddAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task AddRangeAsync(IList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public async Task AddRangeAndSaveAsync(IList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask<IList<TEntity>> AddRangeWithResultAsync(IList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var result = new List<TEntity>();

        foreach (var entity in entities)
        {
            var entry = await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            result.Add(entry.Entity);
        }

        return result;
    }

    public async ValueTask<IList<TEntity>> AddRangeWithResultAndSaveAsync(IList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var result = new List<TEntity>();

        foreach (var entity in entities)
        {
            var entry = await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            result.Add(entry.Entity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return result;
    }
    #endregion

    #region [ Update ]

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(IList<TEntity> entities)
    {
        dbContext.Set<TEntity>().UpdateRange(entities);
    }

    public async Task UpdateAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAndSaveAsync(IList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().UpdateRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region [ Deleting ]

    public void Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async ValueTask<int> SoftDeleteAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Remove(entity);

        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetSingleByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new BadRequestException("Entity Is Not Found");

        Delete(entity);
    }

    public async ValueTask<int> SoftDeleteByIdAndSaveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteByIdAsync(id, cancellationToken);

        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public void SoftDeleteRange(IList<TEntity> entities)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
    }

    public async ValueTask<int> SoftDeleteRangeAndSaveAsync(IList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        SoftDeleteRange(entities);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteRangeByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default)
    {
        var deletingEntities = await GetManyByIdsAsync(ids, cancellationToken);
        if (deletingEntities.IsNotNullOrEmpty() || deletingEntities.Count != ids.Count)
            throw new BadRequestException("Id List is not valid");

        SoftDeleteRange(deletingEntities);
    }

    public async ValueTask<int> SoftDeleteRangeByIdsAndSaveAsync(IList<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        await SoftDeleteRangeByIdsAsync(ids, cancellationToken);

        return await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region [ Others ]
    public IQueryable<TEntity> GetQueryable(bool asTracking = false)
    {
        var result = dbContext.Set<TEntity>().AsQueryable();

        return asTracking ? result.AsTracking() : result;
    }

    public async ValueTask<bool> IsAnyAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        var result = await GetQueryable()
                                .Where(expression)
                                .AnyAsync(cancellationToken);

        return result;
    }
    #endregion
}
