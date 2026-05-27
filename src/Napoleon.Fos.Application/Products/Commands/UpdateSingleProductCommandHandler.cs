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
        if (command.ProductDto.Id is null)
            throw new ArgumentNullException("Id cannot be null");

        var dbEntity = await appUnitOfWork.ProductRepository.GetSingleByIdAsync(command.ProductDto.Id.Value, cancellationToken);

        if (dbEntity is null)
            throw new ArgumentNullException("Not found entity");

        var entity = command.ProductDto.ToEntity();

        await appUnitOfWork.ProductRepository.UpdateAndSaveAsync(entity, cancellationToken);

        return command.ProductDto;
    }
}
