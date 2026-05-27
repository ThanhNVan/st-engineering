using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Domain.Entities;

namespace Napoleon.Fos.Application.Customers.Normal.Extensions;

public static class CustomerMappingExtension
{
    public static Customer ToEntity(this CustomerDto customerDto)
    {
        //return new Customer(customerDto.Id!.Value, customerDto.Name, customerDto.Age, customerDto.Email);
        return new Customer();
    }

    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(customer.Id, customer.Name, customer.Age, customer.Email);
    }
}
