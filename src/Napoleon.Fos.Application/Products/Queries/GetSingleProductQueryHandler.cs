using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.Products.Queries;

public record GetSingleProductQuery(Guid Id) : IQuery<ProductDto>;

public class GetSingleProductQueryHandler(IAppUnitOfWork appUnitOfWork, IMemoryCache cache) : IQueryHandler<GetSingleProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetSingleProductQuery request, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue("products", out IList<ProductDto>? cachedProducts))
        {
            var cachedProduct = cachedProducts.FirstOrDefault(x => x.Id == request.Id);
            if (cachedProduct is not null)
                return cachedProduct;
        }

        var result = await appUnitOfWork.ProductRepository.GetQueryable()
                            .Where(x => x.Id == request.Id)
                            .Include(x => x.ProductDetails)
                            .Select(x => new ProductDto(x.Id, x.Name, x.Description, x.ImageUrl, x.ProductDetails.ToList().ToListDto()))
                            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
