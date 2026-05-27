using MediatR;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.Customers.Normal.Queries;
using Napoleon.Shared.Contract.ApiResponse;
using System.Security.Claims;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/v1/customers")]
public class CustomerController(ISender sender) : ControllerBase
{
    [HttpGet("personal")]
    public async ValueTask<IActionResult> GetPersonalAsync(CancellationToken cancellationToken = default)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var customerId = Guid.Parse(identity.Claims.First(claim => claim.Type == "CustomerId").Value);
        var personalData = await sender.Send(new GetPersonalQuery(customerId), cancellationToken);

        var result = personalData.GetApiResult();

        return Ok(result);
    }
}
