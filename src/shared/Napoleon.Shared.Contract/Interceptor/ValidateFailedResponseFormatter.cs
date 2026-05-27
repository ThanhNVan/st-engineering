using System.Net;
using Microsoft.AspNetCore.Mvc;
using Napoleon.Shared.Common;
using Napoleon.Shared.Contract.ApiResponse;

namespace Napoleon.Shared.Contract.Interceptor;

public class ValidateFailedResponseFormatter
{
    public static IActionResult MakeValidationResponse(ActionContext context)
    {
        var fieldError = new Dictionary<string, IEnumerable<string>>();
        foreach (var keyModelStatePair in context.ModelState)
        {
            var errors = keyModelStatePair.Value.Errors;
           if (errors.IsNotNullOrEmpty())
                fieldError.Add(keyModelStatePair.Key, errors.Select(x => x.ErrorMessage));
        }

        var result = new BadRequestObjectResult( new ApiResult<Dictionary<string, IEnumerable<string>>>(fieldError, HttpStatusCode.BadRequest));

        result.ContentTypes.Add("application/problem+json");

        return result;
    }
}
