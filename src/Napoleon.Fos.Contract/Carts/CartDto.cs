using Napoleon.Fos.Contract.CommonBaseDto;

namespace Napoleon.Fos.Contract.Carts;

public record AddCartDto(Guid? Id, Guid? CustomerId, Guid ProductDetailId, int? Quantity) : BaseDto(Id);
public record CartDto(Guid? Id,
    string Variance,
    string Description,
    string ImageUrl,
    int Price,
    int Quantity,
    Guid ProductId,
    string ProductName,
    string ProductDescription,
    string ProductImageUrl);

public record PersonalCartDto(Guid? Id, IList<CartDto> CartDtos) : BaseDto(Id);