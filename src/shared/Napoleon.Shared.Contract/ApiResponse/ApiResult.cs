using System.Net;

namespace Napoleon.Shared.Contract.ApiResponse;

public record ApiResult<TType>(TType Data, HttpStatusCode HttpStatusCode = HttpStatusCode.OK, string Message = "")
    : BaseApiResult(HttpStatusCode, Message);