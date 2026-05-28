using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.ProductDetails;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.ProductDetails.Queries;

public record GetManyByProductIdQuery(Guid ProductId) : IQuery<IList<ProductDetailDto>>;

public class GetManyByProductIdQueryHandler(IAppUnitOfWork appUnitOfWork) : IQueryHandler<GetManyByProductIdQuery, IList<ProductDetailDto>>
{
    public async Task<IList<ProductDetailDto>> Handle(GetManyByProductIdQuery request, CancellationToken cancellationToken)
    {
        var query = appUnitOfWork.ProductDetailtRepository
                        .GetQueryable()
                        .Where(x => x.ProductId.Equals(request.ProductId))
                        .Select(x => x.ToDto());

        var result = await appUnitOfWork.ProductDetailtRepository.GetManyTypeAsync(query, cancellationToken);
        return result ?? [];
    }
}
