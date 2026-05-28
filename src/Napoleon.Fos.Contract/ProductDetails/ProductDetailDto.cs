using Napoleon.Fos.Contract.CommonBaseDto;

namespace Napoleon.Fos.Contract.ProductDetails;

public record ProductDetailDto(Guid? Id, string Varience, string Description, string ImageUrl, int Price) : BaseDto(Id);
