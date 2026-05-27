using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Napoleon.Shared.Contract.ApiResponse;

namespace Napoleon.Shared.Contract.Interceptor;

public class ApiFormatter : IAsyncResultFilter
{
    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is not ObjectResult result) return next();

        if (result.Value is not null and not BaseApiResult)
            result.Value = result.Value.GetApiResult();

        else if (result.Value is BaseApiResult baseResponse)
            result.StatusCode = (int)baseResponse.HttpStatusCode;

        return next();
    }
}