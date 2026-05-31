using Napoleon.Fos.Contract.Products;

namespace Napoleon.Fos.Application.Products.Commands;

public interface IProductMessageConsumer
{
    Task HandleOrderCreatedAsync(ProductDto message);
}
