using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Products.Commands;

public record AddSingleProductCommand(ProductDto ProductDto) : ICommand<ProductDto>;

public class AddSingleProductCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<AddSingleProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(AddSingleProductCommand command, CancellationToken cancellationToken)
    {
        var entity = command.ProductDto.ToEntity();

        var result = await appUnitOfWork.ProductRepository.AddAndSaveAsync(entity, cancellationToken);

        return result.ToDto();
    }
}