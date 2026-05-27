using Napoleon.Fos.Core.Repositories;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Fos.Infrastructure.AppDbContexts;
using Napoleon.Shared.Infrastructure.Repositories;

namespace Napoleon.Fos.Infrastructure.Repositories;

public class ProductRepository(AppDbContext dbContexts)
    : BaseRepository<Product>(dbContexts), IProductRepository;
