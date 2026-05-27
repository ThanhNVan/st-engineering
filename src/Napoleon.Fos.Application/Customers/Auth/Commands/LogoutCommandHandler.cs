using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Customers.Auth.Commands;

public record LogoutCommand(Guid tokenId) : ICommand<bool>;

public class LogoutCommandhandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(LogoutCommand commend, CancellationToken cancellationToken)
    {
        var result = await appUnitOfWork.AuthenticationTokenRepository.SoftDeleteByIdAndSaveAsync(commend.tokenId, cancellationToken) > 0;
        return result;
    }
}
