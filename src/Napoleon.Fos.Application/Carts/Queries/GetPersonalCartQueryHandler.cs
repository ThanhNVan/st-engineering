using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Contract.Carts;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.Carts.Queries;

public record GetPersonalCartQuery(Guid CustomerId) : IQuery<IList<CartDto>>;

public class GetPersonalCartQueryHandler(IAppUnitOfWork appUnitOfWork) : IQueryHandler<GetPersonalCartQuery, IList<CartDto>>
{
    public async Task<IList<CartDto>> Handle(GetPersonalCartQuery request, CancellationToken cancellationToken)
    {
        var query = appUnitOfWork.CartRepository.GetQueryable()
                                .Where(x => x.CustomerId.Equals(request.CustomerId))
                                    .Include(x => x.ProductDetail)
                                        .ThenInclude(x => x.Product)
                                    .Select(x => new CartDto(x.Id, 
                                                        x.ProductDetail.Varience,
                                                        x.ProductDetail.Description,
                                                        x.ProductDetail.ImageUrl,
                                                        x.ProductDetail.Price,
                                                        x.ProductDetail.Quantity,
                                                        x.ProductDetail.Product.Id,
                                                        x.ProductDetail.Product.Name,
                                                        x.ProductDetail.Product.Description,
                                                        x.ProductDetail.Product.ImageUrl));

        var result = await appUnitOfWork.CartRepository.GetManyTypeAsync(query, cancellationToken);

        return result ?? [];
    }
}