using Microsoft.AspNetCore.Diagnostics;
using Napoleon.Shared.Common.CustomException;
using Napoleon.Shared.Contract.ApiResponse;
using System.Net;

namespace Napoleon.Fos.Presentation.WebApi.Interceptor;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        HttpStatusCode statusCode;
        var message = string.Empty;

        switch (exception)
        {
            case ArgumentNullException:
            case ArgumentException:
                message = "An argument exception occurred.";
                _logger.LogError(exception, "{Message} Exception at UTC: {UtcTime}", message, DateTimeOffset.UtcNow);
                statusCode = HttpStatusCode.BadRequest;
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "You are allowed to process this Api, please sign in to continue.";
                break;

            default:
                message = "An internal server error occurred.";
                _logger.LogError(exception, "{Message} InternalServerError at UTC: {UtcTime}", message, DateTimeOffset.UtcNow);
                statusCode = HttpStatusCode.InternalServerError;
                break;
        }

        var result = new BaseApiResult(statusCode, message);
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}
