using Napoleon.Fos.Core.Repositories;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Infrastructure.AppDbContexts;

namespace Napoleon.Fos.Infrastructure.UnitOfWork;

public class AppUnitOfWork(AppDbContext appDbContext, 
    ICustomerRepository customerRepository, 
    IAuthenticationTokenRepository authenticationTokenRepository, 
    IProductRepository productRepository) 
    : IAppUnitOfWork
{
    public ICustomerRepository CustomerRepository { get; } = customerRepository;
    public IAuthenticationTokenRepository AuthenticationTokenRepository { get; } = authenticationTokenRepository;
    public IProductRepository ProductRepository { get; } = productRepository;

    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
