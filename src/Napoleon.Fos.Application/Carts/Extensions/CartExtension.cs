using Napoleon.Fos.Contract.Carts;
using Napoleon.Fos.Domain.Entities;

namespace Napoleon.Fos.Application.Carts.Extensions;

public static class CartExtension
{
    public static CartDto GetCartDto(this Cart x)
    {
        return new CartDto(x.Id,
                        x.ProductDetail.Varience,
                        x.ProductDetail.Description,
                        x.ProductDetail.ImageUrl,
                        x.ProductDetail.Price,
                        x.Quantity,
                        x.ProductDetail.Product.Id,
                        x.ProductDetail.Product.Name,
                        x.ProductDetail.Product.Description,
                        x.ProductDetail.Product.ImageUrl);

    }
}
