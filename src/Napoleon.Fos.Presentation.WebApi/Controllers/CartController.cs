using MediatR;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.Carts.Commands;
using Napoleon.Fos.Application.Carts.Queries;
using Napoleon.Fos.Contract.Carts;
using Napoleon.Shared.Contract.ApiResponse;
using System.Security.Claims;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/v1/cart")]
public class CartController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    public async ValueTask<IActionResult> GetPersonalCartAsync(CancellationToken cancellationToken = default)
    {
        var isValid = Guid.TryParse(User.FindFirstValue("CustomerId"), out var customerId);
        if (!isValid)
            throw new UnauthorizedAccessException("Invalid CustomerId Claims");

        var result = await sender.Send(new GetPersonalCartQuery(customerId), cancellationToken);
        return Ok(result.GetApiResult());
    }
    
    [HttpPost("")]
    public async ValueTask<IActionResult> AddAsync([FromBody] AddCartDto addCartDto,CancellationToken cancellationToken = default)
    {
        var isValid = Guid.TryParse(User.FindFirstValue("CustomerId"), out var customerId);
        if (!isValid)
            throw new UnauthorizedAccessException("Invalid CustomerId Claims");

        var result = await sender.Send(new AddToCartCommand(new AddCartDto(customerId, addCartDto.ProductDetailId, addCartDto.Quantity)), cancellationToken);
        return Ok(result.GetApiResult());
    }
    
    [HttpDelete("")]
    public async ValueTask<IActionResult> DeleteSingleByIdAsync([FromRoute] Guid Id,CancellationToken cancellationToken = default)
    {
        var isValid = Guid.TryParse(User.FindFirstValue("CustomerId"), out var customerId);
        if (!isValid)
            throw new UnauthorizedAccessException("Invalid CustomerId Claims");

        var result = await sender.Send(new DeleteSingleCommand(Id), cancellationToken);
        return Ok(result.GetApiResult());
    }
    
    [HttpDelete("all")]
    public async ValueTask<IActionResult> DeleteAllByCustomerIdAsync(CancellationToken cancellationToken = default)
    {
        var isValid = Guid.TryParse(User.FindFirstValue("CustomerId"), out var customerId);
        if (!isValid)
            throw new UnauthorizedAccessException("Invalid CustomerId Claims");

        var result = await sender.Send(new DeletePersonalCartCommand(customerId), cancellationToken);
        return Ok(result.GetApiResult());
    }
}
