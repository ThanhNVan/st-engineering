using Napoleon.Fos.Contract.ProductDetails;
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
    
    public static ProductDetail ToEntity(this ProductDetailDto productDetailDto)
    {

        return productDetailDto.Id.HasValue
            ? new ProductDetail
            {
                Id = productDetailDto.Id.Value,
                Description = productDetailDto.Description,
                ImageUrl = productDetailDto.ImageUrl,
                Varience = productDetailDto.Varience,
            } : new ProductDetail
            {
                Description = productDetailDto.Description,
                ImageUrl = productDetailDto.ImageUrl,
                Varience = productDetailDto.Varience,
            }
        ;
    }

    public static List<ProductDetail> ToListEntity(this List<ProductDetailDto> productDetailDtos)
    {
        var result = new List<ProductDetail>();

        foreach(var detail in productDetailDtos)
        {
            result.Add(detail.ToEntity());
        }

        return result;
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Description, product.ImageUrl, product.ProductDetails.ToList().ToListDto());
    }

    public static ProductDetailDto ToDto(this ProductDetail productDetail)
    {
        return new ProductDetailDto(productDetail.Id, productDetail.Varience, productDetail.Description, productDetail.ImageUrl, productDetail.Price);
    }
    
    public static IList<ProductDetailDto> ToListDto(this IList<ProductDetail> productDetailList)
    {
        var result = new List<ProductDetailDto>();

        foreach ( var productDetail in productDetailList)
        {
            result.Add(new ProductDetailDto(productDetail.Id, productDetail.Varience, productDetail.Description, productDetail.ImageUrl, productDetail.Price));

        }

        return result;
    }
}
