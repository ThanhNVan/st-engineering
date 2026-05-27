using Microsoft.AspNetCore.Authorization;
using Napoleon.Fos.Core.Repositories;
using Napoleon.Shared.Common.CustomException;
using System.IdentityModel.Tokens.Jwt;

namespace Napoleon.Fos.Presentation.WebApi.Definitions;

public class CustomAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    public CustomAuthenticationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            // This endpoint allows anonymous access, skip authentication
            await _next(context);
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var authenticationTokenRepository = scope.ServiceProvider.GetRequiredService<IAuthenticationTokenRepository>();

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            throw new UnauthorizedException("failed to validate");
            
        var token = authHeader.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var isValidTokenId = Guid.TryParse(jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value, out var tokenId);
        var isValidCustomerId = Guid.TryParse(jwtSecurityToken.Claims.First(claim => claim.Type == "CustomerId").Value, out var customerId);

        if (!isValidTokenId || !isValidCustomerId)
            throw new UnauthorizedException("failed to validate");


        // Validate the token (you can use JWT or any other token mechanism)
        var isTokenValid = await authenticationTokenRepository.IsAnyAsync(x => x.Id == tokenId 
                                                            && x.ValidTill > DateTimeOffset.Now
                                                            && x.CustomerId == customerId);

        if (!isTokenValid)
            throw new UnauthorizedException("failed to validate");

        await _next(context); // Pass to the next middleware
    }
}
