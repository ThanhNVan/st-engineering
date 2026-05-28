using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.ProductDetails.Commands;

public record DeleteSingleProductDetailCommand(Guid Id) : ICommand<bool>;

public class DeleteSingleProductCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<DeleteSingleProductDetailCommand, bool>
{
    public async Task<bool> Handle(DeleteSingleProductDetailCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command.Id);
        var result = await appUnitOfWork.ProductDetailtRepository.SoftDeleteByIdAndSaveAsync(command.Id, cancellationToken);
        return result > 0;
    }
}
