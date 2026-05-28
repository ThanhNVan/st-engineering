using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Products.Commands;

public record UpdateSingleProductCommand(ProductDto ProductDto) : ICommand<ProductDto>;

public class UpdateSingleProductCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<UpdateSingleProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateSingleProductCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command.ProductDto.Id);
        var query = appUnitOfWork.ProductRepository.GetQueryable()
                                   .Include(x => x.ProductDetails)
                               .Where(x => x.Id.Equals(command.ProductDto.Id));

        var dbEntity = await appUnitOfWork.ProductRepository.GetSingleAsync(query, cancellationToken);

        ArgumentNullException.ThrowIfNull(dbEntity);
        var entity = command.ProductDto.ToEntity();

        appUnitOfWork.ProductRepository.Update(entity);
        await appUnitOfWork.ProductDetailtRepository.UpdateRangeAndSaveAsync([.. entity.ProductDetails], cancellationToken);

        return command.ProductDto;
    }
}
