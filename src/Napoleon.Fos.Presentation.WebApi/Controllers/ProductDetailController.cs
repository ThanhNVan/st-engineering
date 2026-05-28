using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.ProductDetails.Commands;
using Napoleon.Fos.Contract.ProductDetails;
using Napoleon.Shared.Contract.ApiResponse;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/v1/product-details")]
public class ProductDetailController(ISender sender) : ControllerBase
{
    [HttpPost("")]
    public async ValueTask<IActionResult> AddSingleAsync([FromBody] ProductDetailDto productDto, CancellationToken cancellationToken = default)
    {
        var data = await sender.Send(new AddSingleProductDetailCommand(productDto), cancellationToken);

        var result = data.GetApiResult();

        return Ok(result);
    }
}
