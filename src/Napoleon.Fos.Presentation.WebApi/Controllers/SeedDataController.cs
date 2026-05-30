using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.SeedData;
using Swashbuckle.Swagger.Annotations;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/v1/seed-data")]
public class SeedDataController(ISender sender, ICapPublisher _cap) : ControllerBase
{

    [HttpPost("")]
    [AllowAnonymous]
    [SwaggerOperation("Seed data, return a list of email for you to login")]
    public async ValueTask<IActionResult> AddSingleAsync(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new SeedDataCommand(), cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("test-cap")]
    public async ValueTask<IActionResult> TestCapAsync(CancellationToken cancellationToken = default)
    {
        await _cap.PublishAsync("order.created", new
        {
            OrderId = Random.Shared.Next(10, 1_000),
            ProductName = "ProductName",
            Amount = 3
        });

        return Ok("Ok");
    }
}
