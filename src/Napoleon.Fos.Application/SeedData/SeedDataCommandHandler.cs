using Bogus;
using Napoleon.Fos.Application.Extensions;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.SeedData;

public record SeedDataCommand : ICommand<string>;

public class SeedDataCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<SeedDataCommand, string>
{
    public async Task<string> Handle(SeedDataCommand command, CancellationToken cancellationToken)
    {
        var faker = new Faker();

        var customerFaker = new Faker<Customer>()
                .RuleFor(x => x.IsDeleted, f => false)
                .RuleFor(x => x.Age, f => faker.Random.Int(16, 95))
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.Password, f => "12345678".ComputeSHA512())
                .RuleFor(x => x.Email, f => f.Person.Email);
        var customers = customerFaker.Generate(5);

        var productFaker = new Faker<Product>()
                .RuleFor(x => x.IsDeleted, f => false)
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl());
        var products = productFaker.Generate(5);

        await appUnitOfWork.ProductRepository.AddRangeAsync(products, cancellationToken);
        await appUnitOfWork.CustomerRepository.AddRangeAsync(customers, cancellationToken);
        var isSucceeded = await appUnitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!isSucceeded)
            return "Seed data unsucceed.";

        var emailsString = string.Join(", ", customers.Select(x => x.Email));

        return emailsString += "with password: 12345678";
    }
}
