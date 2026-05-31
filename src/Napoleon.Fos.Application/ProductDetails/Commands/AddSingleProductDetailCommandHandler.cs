using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.ProductDetails;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.ProductDetails.Commands;

public record AddSingleProductDetailCommand(ProductDetailDto ProductDetailDto) : ICommand<ProductDetailDto>;

public class AddSingleProductDetailHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<AddSingleProductDetailCommand, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(AddSingleProductDetailCommand command, CancellationToken cancellationToken)
    {
        var entity = command.ProductDetailDto.ToEntity();
        var dbProduct = await appUnitOfWork.ProductRepository.GetSingleByIdAsync(command.ProductDetailDto.ProductId.Value);
        ArgumentNullException.ThrowIfNull(dbProduct);

        var dbEntity = await appUnitOfWork.ProductDetailtRepository.AddAndSaveAsync(entity, cancellationToken);

        return dbEntity.ToDto();
    }
}
