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
                ProductDetails = productDto.ProductDetails.ToList().ToListEntity()
            } : new Product
            {
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                Name = productDto.Name,
                ProductDetails = productDto.ProductDetails.ToList().ToListEntity()
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
                Varience = productDetailDto.Variance,
                Price = productDetailDto.Price,
                ProductId = productDetailDto.ProductId,
                Quantity = productDetailDto.Quantity
            } : new ProductDetail
            {
                Description = productDetailDto.Description,
                ImageUrl = productDetailDto.ImageUrl,
                Varience = productDetailDto.Variance,
                Price = productDetailDto.Price,
                ProductId = productDetailDto.ProductId,
                Quantity = productDetailDto.Quantity
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
        return new ProductDetailDto(productDetail.Id, productDetail.Varience, productDetail.Description, productDetail.ImageUrl, productDetail.Price, productDetail.ProductId, productDetail.Quantity);
    }
    
    public static IList<ProductDetailDto> ToListDto(this IList<ProductDetail> productDetailList)
    {
        var result = new List<ProductDetailDto>();

        foreach ( var productDetail in productDetailList)
        {
            result.Add(new ProductDetailDto(productDetail.Id, productDetail.Varience, productDetail.Description, productDetail.ImageUrl, productDetail.Price, productDetail.ProductId, productDetail.Quantity));

        }

        return result;
    }
}
