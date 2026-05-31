using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Common;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.Products.Queries;

public record GetAllProductQuery(string? Name) : IQuery<IList<ProductDto>>;

public class GetAllProductQueryHandler(IAppUnitOfWork appUnitOfWork, IMemoryCache cache) : IQueryHandler<GetAllProductQuery, IList<ProductDto>>
{
    public async Task<IList<ProductDto>> Handle(GetAllProductQuery queryRequest, CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync("products", async entry =>
        {
            var query = queryRequest.Name.IsNullOrEmpty()
                       ? appUnitOfWork.ProductRepository.GetQueryable().Include(x => x.ProductDetails.Where(x => x.Quantity > 0))
                       // note: must use ToLower, don't use stringComparison
                       : appUnitOfWork.ProductRepository.GetQueryable().Include(x => x.ProductDetails.Where(x => x.Quantity > 0))
                               .Where(x => x.Name.Trim().ToLower().Contains(queryRequest.Name.ToLower()));

            var queryDto = query.Select(x => new ProductDto(x.Id, x.Name, x.Description, x.ImageUrl, x.ProductDetails.ToList().ToListDto()));

            var result = await appUnitOfWork.ProductRepository.GetManyTypeAsync(queryDto, cancellationToken);

            return result ?? [];
        });
    }
}
