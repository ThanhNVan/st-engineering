using DotNetCore.CAP;
using Napoleon.Fos.Contract.Products;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Products.Commands;

public record AddSingleProductCommand(ProductDto ProductDto) : ICommand<bool>;

public class AddSingleProductCommandHandler(ICapPublisher capPublisher) : ICommandHandler<AddSingleProductCommand, bool>
{
    public async Task<bool> Handle(AddSingleProductCommand command, CancellationToken cancellationToken)
    {
        await capPublisher.PublishAsync("AddProduct", command.ProductDto);

        return true;
    }
}