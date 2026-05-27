using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ApplicationUnitTest.Helper;
public static class AsyncQueryable
{
    /// <summary>
    /// Returns the input typed as IQueryable that can be queried asynchronously
    /// </summary>
    /// <typeparam name="TEntity">The item type</typeparam>
    /// <param name="source">The input</param>
    public static IQueryable<TEntity> AsAsyncQueryable<TEntity>(this IEnumerable<TEntity> source)
    => new AsyncQueryable<TEntity>(source ?? throw new ArgumentNullException(nameof(source)));
}

public class AsyncQueryable<TEntity> : EnumerableQuery<TEntity>, IAsyncEnumerable<TEntity>, IQueryable<TEntity>
{
    public AsyncQueryable(IEnumerable<TEntity> enumerable) : base(enumerable) { }
    public AsyncQueryable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<TEntity> GetEnumerator() => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
    IQueryProvider IQueryable.Provider => new AsyncQueryProvider(this);

    class AsyncEnumerator(IEnumerator<TEntity> inner) : IAsyncEnumerator<TEntity>
    {
        public TEntity Current => inner.Current;
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(inner.MoveNext());
        public async ValueTask DisposeAsync() => await Task.Factory.StartNew(inner.Dispose);
    }

    class AsyncQueryProvider : IAsyncQueryProvider
    {
        internal AsyncQueryProvider(IQueryProvider inner) => this._inner = inner;

        private readonly IQueryProvider _inner;
        public IQueryable CreateQuery(Expression expression) => new AsyncQueryable<TEntity>(expression);
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new AsyncQueryable<TElement>(expression);
        public object? Execute(Expression expression) => _inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Execute<TResult>(expression);
    }
}
