using Napoleon.Fos.Core.Repositories;

namespace Napoleon.Fos.Core.UnitOfWork;

public interface IAppUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }

    IAuthenticationTokenRepository AuthenticationTokenRepository { get; }

    IProductRepository ProductRepository { get; }

    IProductDetailtRepository ProductDetailtRepository { get; }
    ICartRepository CartRepository { get; }

    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
