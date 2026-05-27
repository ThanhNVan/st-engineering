using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Fos.Application.Customers.Auth.Commands;
using Napoleon.Fos.Contract.Customers;
using Napoleon.Shared.Contract.ApiResponse;
using System.Security.Claims;

namespace Napoleon.Fos.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class CustomerAuthController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async ValueTask<IActionResult> RegisterAsync([FromBody] CustomerAuthDto customeAuthDto,
       CancellationToken cancellationToken = default)
    {
        var isRegisterSucceed = await sender.Send(new RegisterCommand(customeAuthDto), cancellationToken);
        var result = isRegisterSucceed.GetApiResult(message: isRegisterSucceed ? "Register Successfully" : "Email is existed");

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async ValueTask<IActionResult> LoginAsync([FromBody] CustomerAuthDto customeAuthDto,
       CancellationToken cancellationToken = default)
    {
        var jwtToken = await sender.Send(new LoginCommand(customeAuthDto), cancellationToken);

        if (jwtToken == string.Empty)
            return Unauthorized("wrong password or email");

        var result = jwtToken.GetApiResult();

        return Ok(result);
    }

    [HttpDelete("logout")]
    public async ValueTask<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var tokenId = Guid.Parse(identity.Claims.First(claim => claim.Type == "Id").Value);
        var isLogoutSucceed = await sender.Send(new LogoutCommand(tokenId), cancellationToken);

        if (!isLogoutSucceed)
            return BadRequest("Log out fail.");

        var result = isLogoutSucceed.GetApiResult();

        return Ok(result);
    }
}
