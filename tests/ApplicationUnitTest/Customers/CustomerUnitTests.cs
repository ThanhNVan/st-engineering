using ApplicationUnitTest.Helper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Moq;
using Napoleon.Fos.Application.Customers.Normal.Extensions;
using Napoleon.Fos.Application.Customers.Normal.Queries;
using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;

namespace ApplicationUnitTest.Customers;

public class CustomerUnitTests
{
    private readonly Mock<IAppUnitOfWork> _appUnitOfWork = new ();

    [Fact]
    public async Task GetPersonalQuery_Return_Success()
    {
        // Arrange
        var customers = GetSeedCustomerData(1);
        var customerQueryable = customers.AsAsyncQueryable();
        var expected = customers.FirstOrDefault()!;
        _appUnitOfWork.Setup(x => x.CustomerRepository.GetQueryable(It.IsAny<QueryTrackingBehavior>())).Returns(customerQueryable);
        _appUnitOfWork.Setup(x => x.CustomerRepository.GetSingleTypeAsync(It.IsAny<IQueryable<CustomerDto>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected.ToDto());

        // Act
        var handler = new GetPersonalQueryHandler(_appUnitOfWork.Object);
        var actual = await handler.Handle(new GetPersonalQuery(customers.FirstOrDefault().Id), CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(actual.Id, expected.Id);

    }

    private IList<Customer> GetSeedCustomerData(int numberOfEntity)
    {
        var faker = new Faker();
        var customerFaker = new Faker<Customer>()
               .RuleFor(x => x.IsDeleted, f => false)
               .RuleFor(x => x.Age, f => faker.Random.Int(16, 95))
               .RuleFor(x => x.Name, f => f.Person.FullName)
               .RuleFor(x => x.Password, f => "12345678".ComputeSHA512())
               .RuleFor(x => x.Email, f => f.Person.Email);
        var customers = customerFaker.Generate(numberOfEntity);

        return customers;
    }
}
