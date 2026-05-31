using DotNetCore.CAP;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;

namespace Napoleon.Fos.Application.Products.Commands;

public class ProductMessageConsumer(IAppUnitOfWork appUnitOfWork) : IProductMessageConsumer, ICapSubscribe
{
    // Subscribes to the "order.created" event
    [CapSubscribe("AddProduct", Group = "default")]
    public async Task HandleOrderCreatedAsync(ProductDto message)
    {
        Console.WriteLine(message);
        var entity = message.ToEntity();
        //entity.Id = Ulid.NewUlid().ToGuid();
        var details = message.ProductDetails?.ToList().ToListEntity();
        foreach (var detail in details)
        {
            detail.ProductId = entity.Id;
            detail.Id = Ulid.NewUlid().ToGuid();
        }

        var dbEntity = await appUnitOfWork.ProductRepository.AddAsync(entity);
        var dbDetails = await appUnitOfWork.ProductDetailtRepository.AddRangeWithResultAsync(details);

        await appUnitOfWork.SaveChangesAsync();
        
    }
}
