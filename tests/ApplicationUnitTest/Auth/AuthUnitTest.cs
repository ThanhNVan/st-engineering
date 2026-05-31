using ApplicationUnitTest.Helper;
using AutoFixture;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Moq;
using Napoleon.Fos.Application.Customers.Auth.Commands;
using Napoleon.Fos.Application.Customers.Normal.Extensions;
using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Fos.Core.Settings;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;

namespace ApplicationUnitTest.Auth;

public class AuthUnitTest
{
    private readonly Mock<IAppUnitOfWork> _appUnitOfWork = new();
   private readonly Fixture _fixture = new();

    [Fact]
    public async Task GetPersonalQuery_Return_Success()
    {
        // Arrange
        var customers = GetSeedCustomerData(1);
        var queryable = customers.AsAsyncQueryable();
        var expected = customers.FirstOrDefault()!;
        var jwtSettings = _fixture.Create<JwtSettings>();
        var token = expected.ToDto().GetCustomerAccessToken(jwtSettings);
        var authenticationToken = new AuthenticationToken
        {
            Id = token.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            CustomerId = expected.Id,
            IsDeleted = false,
            Token = token.Token,
            ValidTill = DateTimeOffset.UtcNow.AddDays(jwtSettings.ValidationLengthInDays),
        };

        _appUnitOfWork.Setup(x => x.AuthenticationTokenRepository.AddAndSaveAsync(It.IsAny<AuthenticationToken>(), It.IsAny<CancellationToken>())).ReturnsAsync(authenticationToken);
        _appUnitOfWork.Setup(x => x.CustomerRepository.GetSingleTypeAsync(It.IsAny<IQueryable<CustomerDto>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected.ToDto());
        _appUnitOfWork.Setup(x => x.CustomerRepository.GetQueryable(It.IsAny<QueryTrackingBehavior>())).Returns(queryable);

        // Act
        var handler = new LoginCommandHandler(_appUnitOfWork.Object, jwtSettings);
        var actual = await handler.Handle(new LoginCommand(new CustomerAuthDto(expected.Email, "12345678")), CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(actual, token.Token);
    }

    [Fact]
    public async Task LogoutCommand_Return_Success()
    {
        // Arrange

        _appUnitOfWork.Setup(x => x.AuthenticationTokenRepository.SoftDeleteByIdAndSaveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var handler = new LogoutCommandhandler(_appUnitOfWork.Object);
        var actual = await handler.Handle(new LogoutCommand(Ulid.NewUlid().ToGuid()), CancellationToken.None);
        // Assert

        Assert.True(actual);
    }
    
    [Fact]
    public async Task RegisterCommand_Return_Success()
    {
        // Arrange
        var customers = GetSeedCustomerData(1);
        var expected = customers.FirstOrDefault()!;
        var customerAuthDto = new CustomerAuthDto(expected.Email, "12345678");

        _appUnitOfWork.Setup(x => x.CustomerRepository.AddAndSaveAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var handler = new RegisterCommandHanlder(_appUnitOfWork.Object);
        var actual = await handler.Handle(new RegisterCommand(customerAuthDto), CancellationToken.None);
        // Assert

        Assert.True(actual);
    }

    private static IList<Customer> GetSeedCustomerData(int numberOfEntity)
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
