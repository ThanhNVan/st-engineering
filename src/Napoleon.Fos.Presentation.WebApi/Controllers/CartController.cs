using MediatR;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.Carts.Queries;
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
}
