using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.ProductDetails;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.ProductDetails.Commands;

public record UpdateSingleProductDetailCommand(ProductDetailDto ProductDetailDto) : ICommand<ProductDetailDto>;

public class UpdateSingleProductDetailHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<UpdateSingleProductDetailCommand, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(UpdateSingleProductDetailCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command.ProductDetailDto.Id);
        ArgumentNullException.ThrowIfNull(command.ProductDetailDto.ProductId);
        
        var dbProduct = await appUnitOfWork.ProductRepository.GetSingleByIdAsync(command.ProductDetailDto.ProductId, cancellationToken);
        ArgumentNullException.ThrowIfNull(dbProduct);
        
        await appUnitOfWork.ProductDetailtRepository.UpdateAndSaveAsync(command.ProductDetailDto.ToEntity(), cancellationToken);
        var dbEntity = await appUnitOfWork.ProductDetailtRepository.GetSingleByIdAsync(command.ProductDetailDto.Id.Value, cancellationToken);

        return dbEntity.ToDto();
    }
}