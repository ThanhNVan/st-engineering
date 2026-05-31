using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.Products.Queries;

public record GetSingleProductQuery(Guid Id) : IQuery<ProductDto>;

public class GetSingleProductQueryHandler(IAppUnitOfWork appUnitOfWork) : IQueryHandler<GetSingleProductQuery, ProductDto>
{
    public Task<ProductDto> Handle(GetSingleProductQuery request, CancellationToken cancellationToken)
    {
        var result = appUnitOfWork.ProductRepository.GetQueryable()
                            .Where(x => x.Id == request.Id)
                            .Include(x => x.ProductDetails)
                            .Select(x => new ProductDto(x.Id, x.Name, x.Description, x.ImageUrl, x.ProductDetails.ToList().ToListDto()))
                            .FirstOrDefaultAsync(cancellationToken);
        return result;
    }
}
