using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Queries;

namespace Napoleon.Fos.Application.Customers.Normal.Queries;

public record GetPersonalQuery(Guid CustomerId): IQuery<CustomerDto>;

public class GetPersonalQueryHandler(IAppUnitOfWork appUnitOfWork)
    : IQueryHandler<GetPersonalQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetPersonalQuery request, CancellationToken cancellationToken)
    {
        var query = appUnitOfWork.CustomerRepository.GetQueryable()
            .Where(x => x.Id == request.CustomerId)
            .Select(x => new CustomerDto(x.Id, x.Name, x.Age, x.Email));

        var result = await appUnitOfWork.CustomerRepository.GetSingleTypeAsync(query, cancellationToken);

        return result;
    }
}
