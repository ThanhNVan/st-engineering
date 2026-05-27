using System.Net;

namespace Napoleon.Shared.Contract.ApiResponse;

public static class ResultHelper
{
    public static PagingResult<TType> GetPagingResult<TType>(this IEnumerable<TType> items, int totalItem, int pageSize,
        int currentPage)
        where TType : class
    {
        return new PagingResult<TType>(items, totalItem, pageSize, currentPage);
    }

    public static ApiResult<TType> GetApiResult<TType>(this TType data, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "")
    {
        var result = new ApiResult<TType>(data, statusCode, message);

        return result;
    }
}

