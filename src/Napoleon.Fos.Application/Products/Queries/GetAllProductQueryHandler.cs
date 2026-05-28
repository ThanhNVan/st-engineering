using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.Products.Queries;

public record GetAllProductQuery(string? Name) : IQuery<IList<ProductDto>>;

public class GetAllProductQueryHandler(IAppUnitOfWork appUnitOfWork) : IQueryHandler<GetAllProductQuery, IList<ProductDto>>
{
    public async Task<IList<ProductDto>> Handle(GetAllProductQuery queryRequest, CancellationToken cancellationToken)
    {
        var query = appUnitOfWork.ProductRepository.GetQueryable()
                        .Include(x => x.ProductDetails)
                        // note: must use ToLower, don't use stringComparison
                        .Where(x => x.Name.Trim().ToLower().Contains(queryRequest.Name.ToLower()));

        var queryDto = query.Select(x => new ProductDto(x.Id, x.Name, x.Description, x.ImageUrl, x.ProductDetails.ToList().ToListDto()));

        var result = await appUnitOfWork.ProductRepository.GetManyTypeAsync(queryDto, cancellationToken);

        return result ?? [];

    }
}
