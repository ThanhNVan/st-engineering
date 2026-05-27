using Microsoft.AspNetCore.Mvc;
using Napoleon.Shared.Contract.Interceptor;

namespace Napoleon.Shared.Contract.ServiceExtension;
public static class FormatApiResponseFilterExtension
{
    public static void AddApiFormatter(this MvcOptions options) => options.Filters.Add(new ApiFormatter());
}