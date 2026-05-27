using Napoleon.Fos.Contract.CommonBaseDto;

namespace Napoleon.Fos.Contract.Customers;

public record CustomerDto(Guid? Id, string Name, int Age, string Email) : BaseDto(Id);
