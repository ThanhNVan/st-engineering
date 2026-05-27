using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Fos.Infrastructure.AppDbContexts;
using Napoleon.Fos.Infrastructure.UnitOfWork;
//using ProgramClass = Napoleon.Fos.

namespace ControllerIntegrationTest.Helper;

public class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    protected readonly WebApplicationFactory<Program> _appFactory;
    protected IAppUnitOfWork _appUnitOfWork;

    protected readonly List<Product> ProductList = GetSeedProductData(5);
    protected readonly List<Customer> CustomerList = GetSeedCustomerData(5);

    public BaseIntegrationTest(CustomWebApplicationFactory<Program> appFactory)
    {
        _appFactory = appFactory;
       
    }

    protected async Task PrepareDataAsync()
    {
        using var scope = _appFactory.Services.CreateScope();
        _appUnitOfWork = scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>();

        await _appUnitOfWork.ProductRepository.AddRangeAsync(ProductList);
        await _appUnitOfWork.CustomerRepository.AddRangeAsync(CustomerList);
        await _appUnitOfWork.SaveChangesAsync();
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
       // return ResetDatabaseAsync();
    }

    private static List<Product> GetSeedProductData(int numberOfEntity)
    {
        var faker = new Faker();
        var ProductFaker = new Faker<Product>()
               .RuleFor(x => x.IsDeleted, f => false)
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl());
        var Products = ProductFaker.Generate(numberOfEntity);

        return Products;
    }

    private static List<Customer> GetSeedCustomerData(int numberOfEntity)
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

    public async Task ResetDatabaseAsync()
    {
        using var scope = _appFactory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<AppDbContext>();
        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<Program>>>();

        try
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
        } catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred resetting the database. Error: {exceptionMessage}", ex.Message);
        }
    }
}
