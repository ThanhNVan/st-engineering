using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.SeedData;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/v1/seed-data")]
public class SeedDataController(ISender sender) : ControllerBase
{

    [HttpPost("")]
    [AllowAnonymous]
   // [SwaggerOperation(Summary = "Seed data, return a list of email for you to login")]
    public async ValueTask<IActionResult> AddSingleAsync(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new SeedDataCommand(), cancellationToken);

        return Ok(result);
    }
}
