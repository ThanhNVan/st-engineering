using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.Products.Commands;
using Napoleon.Fos.Application.Products.Queries;
using Napoleon.Fos.Contract.Products;
using Napoleon.Shared.Contract.ApiResponse;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

//[AllowAnonymous]
[ApiController]
[Route("api/v1/products")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    public async ValueTask<IActionResult> GetManyAsync([FromQuery] string? name, CancellationToken cancellationToken = default)
    {
        var data = await sender.Send(new GetAllProductQuery(name), cancellationToken);

        var result = data.GetApiResult();

        return Ok(result);
    }
    
    [HttpPost("")]
    public async ValueTask<IActionResult> AddSingleAsync([FromBody] ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var data = await sender.Send(new AddSingleProductCommand(productDto), cancellationToken);

        var result = data.GetApiResult();

        return Ok(result);
    }
    
    [HttpPut("")]
    public async ValueTask<IActionResult> UpdateSingleAsync([FromBody] ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var data = await sender.Send(new UpdateSingleProductCommand(productDto), cancellationToken);

        var result = data.GetApiResult();

        return Ok(result);
    }
    
    [HttpDelete("{productId:guid}")]
    public async ValueTask<IActionResult> DeleteSingleAsync([FromRoute] Guid productId, CancellationToken cancellationToken = default)
    {
        var data = await sender.Send(new DeleteSingleProductCommand(productId), cancellationToken);

        var result = data.GetApiResult();

        return Ok(result);
    }
    
    [HttpGet("{productId:guid}")]
    public async ValueTask<IActionResult> GetSingleAsync([FromRoute] Guid productId, CancellationToken cancellationToken = default)
    {
        var data = await sender.Send(new GetSingleProductQuery(productId), cancellationToken);

        var result = data.GetApiResult();

        return Ok(result);
    }
}
