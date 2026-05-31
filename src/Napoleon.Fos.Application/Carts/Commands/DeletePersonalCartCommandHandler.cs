using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Carts.Commands;

public record DeletePersonalCartCommand(Guid CustomerId) : ICommand<bool>;

public class DeletePersonalCartCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<DeletePersonalCartCommand, bool>
{
    public async Task<bool> Handle(DeletePersonalCartCommand request, CancellationToken cancellationToken)
    {
        var dbEntities = await appUnitOfWork.CartRepository.GetQueryable(QueryTrackingBehavior.TrackAll)
                               .Where(x => x.CustomerId.Equals(request.CustomerId))
                               .ToListAsync(cancellationToken);

        var result = await appUnitOfWork.CartRepository.SoftDeleteRangeAndSaveAsync(dbEntities, cancellationToken);

        return result > 0;
    }
}
