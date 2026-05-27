using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Products.Commands;

public record DeleteSingleProductCommand(Guid ProductId) : ICommand<bool>;

public class DeleteSingleProductCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<DeleteSingleProductCommand, bool>
{
    public async Task<bool> Handle(DeleteSingleProductCommand command, CancellationToken cancellationToken)
    {
        var dbEntity = await appUnitOfWork.ProductRepository.GetSingleByIdAsync(command.ProductId, cancellationToken);

        if (dbEntity is null)
            throw new ArgumentNullException("Not found entity");

        var result = await appUnitOfWork.ProductRepository.SoftDeleteByIdAndSaveAsync(command.ProductId, cancellationToken);

        return result > 0;
    }
}
