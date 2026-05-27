using Napoleon.Fos.Contract.CommonBaseDto;

namespace Napoleon.Fos.Contract.Products;

public record ProductDto(Guid? Id, string Name, string Description, string ImageUrl) : BaseDto(Id);

