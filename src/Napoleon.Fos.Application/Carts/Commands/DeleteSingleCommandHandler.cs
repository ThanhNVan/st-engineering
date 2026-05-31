using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Carts.Commands;

public record DeleteSingleCommand(Guid Id) : ICommand<bool>;

public class DeleteSingleCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<DeleteSingleCommand, bool>
{
    public async Task<bool> Handle(DeleteSingleCommand request, CancellationToken cancellationToken)
    {
        var dbEntity = await appUnitOfWork.CartRepository.GetSingleByIdAsync(request.Id,cancellationToken);
        ArgumentNullException.ThrowIfNull(dbEntity);

        var result = await appUnitOfWork.CartRepository.SoftDeleteAndSaveAsync(dbEntity);

        return result > 0;
    }
}
