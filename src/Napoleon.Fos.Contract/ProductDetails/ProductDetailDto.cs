using Napoleon.Fos.Contract.CommonBaseDto;

namespace Napoleon.Fos.Contract.ProductDetails;

public record ProductDetailDto(Guid? Id, 
    string Variance, 
    string Description, 
    string ImageUrl, 
    int Price,
    Guid ProductId) : BaseDto(Id);
