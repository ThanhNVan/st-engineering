using DotNetCore.CAP;
using Microsoft.Extensions.Caching.Memory;
using Napoleon.Fos.Application.Products.Extensions;
using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Core.UnitOfWork;

namespace Napoleon.Fos.Application.Products.Commands;

public class ProductMessageConsumer(IAppUnitOfWork appUnitOfWork, IMemoryCache cache) : IProductMessageConsumer, ICapSubscribe
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

        if (cache.TryGetValue("products", out IList<ProductDto>? cachedProducts) && cachedProducts is not null)
        {
            var productDto = new ProductDto(dbEntity.Id, dbEntity.Name, dbEntity.Description, dbEntity.ImageUrl, dbDetails.ToListDto());
            cachedProducts.Add(productDto);

            cachedProducts = cachedProducts.DistinctBy(x=> x.Id).ToList();
        }


        cache.Set("products", cachedProducts);

        await appUnitOfWork.SaveChangesAsync();

    }
}
