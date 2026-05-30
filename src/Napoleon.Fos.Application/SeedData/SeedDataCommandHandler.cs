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

        var productDetails = new List<ProductDetail>();
        var productDetailsFaker = new Faker<ProductDetail>()
                .RuleFor(x => x.IsDeleted, f => false)
                .RuleFor(x => x.Varience, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(x => x.Price, f => f.Random.Int(100, 5000))
                .RuleFor(x => x.Quantity, f => f.Random.Int(0, 10));

        foreach (var product in products)
        {
            var details = productDetailsFaker.Generate(5);
            foreach(var detail in details)
            {
                detail.ProductId = product.Id;
            }
            productDetails.AddRange(details);
        }

        var cartFaker = new Faker<Cart>()
            .RuleFor(x => x.IsDeleted, f => false)
            .RuleFor(x => x.Quantity, 1);

        var carts = cartFaker.Generate(25);
        foreach(var (customer, index) in customers.Select((value, i) => (value, i)))
        {
            for (int i = index * 5; i < (index * 5) + 5; i++)
            {
                carts[i].ProductDetailId = productDetails[i].Id;
                carts[i].CustomerId = customer.Id;
            }
        }

        await appUnitOfWork.ProductRepository.AddRangeAsync(products, cancellationToken);
        await appUnitOfWork.CustomerRepository.AddRangeAsync(customers, cancellationToken);
        await appUnitOfWork.ProductDetailtRepository.AddRangeAsync(productDetails, cancellationToken);
        await appUnitOfWork.CartRepository.AddRangeAsync(carts, cancellationToken);
        var isSucceeded = await appUnitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!isSucceeded)
            return "Seed data unsucceed.";

        var emailsString = string.Join(", ", customers.Select(x => string.Join(x.Email, " with password: 12345678")));

        return emailsString;
    }
}
