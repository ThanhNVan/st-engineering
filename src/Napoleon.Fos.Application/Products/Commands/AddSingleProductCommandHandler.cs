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
        entity.Id = Ulid.NewUlid().ToGuid();
        var details = command.ProductDto.ProductDetails.ToList().ToListEntity();
        foreach (var detail in details)
        { 
            detail.ProductId = entity.Id; 
            detail.Id = Ulid.NewUlid().ToGuid();
        }

        var dbEntity = await appUnitOfWork.ProductRepository.AddAsync(entity, cancellationToken);
        var dbDetails=  await appUnitOfWork.ProductDetailtRepository.AddRangeWithResultAsync(details, cancellationToken);
        
        await appUnitOfWork.SaveChangesAsync(cancellationToken);
        var result = new ProductDto(dbEntity.Id, dbEntity.Name, dbEntity.Description, dbEntity.ImageUrl, dbDetails.ToList().ToListDto());
        return result;
    }
}