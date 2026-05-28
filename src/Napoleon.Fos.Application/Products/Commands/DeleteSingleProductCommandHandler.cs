using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Products.Commands;

public record DeleteSingleProductCommand(Guid ProductId) : ICommand<bool>;

// delete a product will also delete its product details
public class DeleteSingleProductCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<DeleteSingleProductCommand, bool>
{
    public async Task<bool> Handle(DeleteSingleProductCommand command, CancellationToken cancellationToken)
    {
        var query = appUnitOfWork.ProductRepository.GetQueryable()
                                    .Include(x => x.ProductDetails)
                                .Where(x => x.Id.Equals(command.ProductId));

        var dbEntity = await appUnitOfWork.ProductRepository.GetSingleAsync(query, cancellationToken);
        ArgumentNullException.ThrowIfNull(dbEntity);

        var productDetailIdList = dbEntity.ProductDetails?
                                        .Select(x => x.Id)
                                        .ToList();

        await appUnitOfWork.ProductRepository.DeleteByIdAsync(command.ProductId, cancellationToken);
        var result = await appUnitOfWork.ProductDetailtRepository.SoftDeleteRangeByIdsAndSaveAsync(productDetailIdList, cancellationToken);


        return result > 0;
    }
}
