using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Napoleon.Shared.Domain.Entity;

namespace Napoleon.Shared.Core.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    #region [ Get ]
    ValueTask<IList<TEntity>?> GetManyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    
    ValueTask<IList<TEntity>?> GetManyByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default);

    ValueTask<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<TEntity?> GetSingleAsync(IQueryable<TEntity> expression, CancellationToken cancellationToken = default);
    ValueTask<TEntity?> GetSingleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    ValueTask<TType?> GetSingleTypeAsync<TType>(IQueryable<TType> queryable, CancellationToken cancellationToken = default);

    ValueTask<IList<TType>?> GetManyTypeAsync<TType>(IQueryable<TType> queryable, CancellationToken cancellationToken = default);
    #endregion

    #region [ Add ]
    ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    ValueTask<TEntity> AddAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

    ValueTask<IList<TEntity>> AddRangeWithResultAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    ValueTask<IList<TEntity>> AddRangeWithResultAndSaveAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task AddRangeAndSaveAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    #endregion

    #region [ Update ]
    void Update(TEntity entity);

    void UpdateRange(IList<TEntity> entities);

    Task UpdateAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAndSaveAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    #endregion

    #region [ Deleting ]
    void Delete(TEntity entity);

    ValueTask<int> SoftDeleteAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    ValueTask<int> SoftDeleteByIdAndSaveAsync(Guid id, CancellationToken cancellationToken = default);

    void SoftDeleteRange(IList<TEntity> entities);

    ValueTask<int> SoftDeleteRangeAndSaveAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    
    Task SoftDeleteRangeByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default);

    ValueTask<int> SoftDeleteRangeByIdsAndSaveAsync(IList<Guid> ids, CancellationToken cancellationToken = default);
    #endregion

    #region [ Others ]
    IQueryable<TEntity> GetQueryable(QueryTrackingBehavior asTracking = QueryTrackingBehavior.NoTracking);

    ValueTask<bool> IsAnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    #endregion
} 
