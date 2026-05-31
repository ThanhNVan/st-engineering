using Microsoft.EntityFrameworkCore;
using Napoleon.Fos.Application.Carts.Extensions;
using Napoleon.Fos.Contract.Carts;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Domain.Entities;
using Napoleon.Shared.Core.Requests.Commands;

namespace Napoleon.Fos.Application.Carts.Commands;

public record AddToCartCommand(AddCartDto AddCartDto) : ICommand<CartDto>;

public class AddToCartCommandHandler(IAppUnitOfWork appUnitOfWork) : ICommandHandler<AddToCartCommand, CartDto>
{
    public async Task<CartDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var dbCart = await appUnitOfWork.CartRepository.GetQueryable()
                                .Include(x => x.ProductDetail)
                                        .ThenInclude(x => x.Product)
                                .FirstOrDefaultAsync(x => x.CustomerId == request.AddCartDto.CustomerId
                                    && x.ProductDetailId == request.AddCartDto.ProductDetailId, cancellationToken);
                                //.Select(x => x.GetCartDto())
                                //.FirstOrDefaultAsync();

        if (dbCart is null)
        {
            var newCart = new Cart
            {
                CustomerId = request.AddCartDto.CustomerId,
                ProductDetailId = request.AddCartDto.ProductDetailId,
                Quantity = 1
            };

            await appUnitOfWork.CartRepository.AddAndSaveAsync(newCart, cancellationToken);
            var result = await appUnitOfWork.CartRepository.GetQueryable()
                                .Include(x => x.ProductDetail)
                                        .ThenInclude(x => x.Product)
                                .Where(x => x.CustomerId == request.AddCartDto.CustomerId
                                    && x.ProductDetailId == request.AddCartDto.ProductDetailId)
                                .Select(x => x.GetCartDto())
                                .FirstOrDefaultAsync();
            return result;
        }
        if (request.AddCartDto.Quantity is not null)
            dbCart.Quantity += request.AddCartDto.Quantity.Value;
        else 
            dbCart.Quantity += 1;

        appUnitOfWork.CartRepository.Update(dbCart);
        await appUnitOfWork.SaveChangesAsync(cancellationToken);
        return dbCart.GetCartDto();
    }
}