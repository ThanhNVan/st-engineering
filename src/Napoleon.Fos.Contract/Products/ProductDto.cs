using Napoleon.Fos.Contract.CommonBaseDto;
using Napoleon.Fos.Contract.ProductDetails;

namespace Napoleon.Fos.Contract.Products;

public record ProductDto(Guid? Id, string Name, string Description, string ImageUrl, IList<ProductDetailDto>? ProductDetails) : BaseDto(Id);

