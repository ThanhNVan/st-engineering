using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Napoleon.Fos.Application.Extensions;

public static class ServiceExtension
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}
