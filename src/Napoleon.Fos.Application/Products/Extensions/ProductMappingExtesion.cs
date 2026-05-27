using Napoleon.Fos.Contract.Products;
using Napoleon.Fos.Domain.Entities;

namespace Napoleon.Fos.Application.Products.Extensions;

public static class ProductMappingExtesion
{
    public static Product ToEntity(this ProductDto productDto)
    {

        return productDto.Id.HasValue
            ? new Product
            {
                Id = productDto.Id.Value,
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                Name = productDto.Name,
            } : new Product
            {
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                Name = productDto.Name,
            }
        ;
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Description, product.ImageUrl);
    }
}
