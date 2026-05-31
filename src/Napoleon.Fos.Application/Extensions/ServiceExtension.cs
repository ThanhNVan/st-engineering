using Microsoft.Extensions.DependencyInjection;
using Napoleon.Fos.Application.Products.Commands;
using System.Reflection;

namespace Napoleon.Fos.Application.Extensions;

public static class ServiceExtension
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IProductMessageConsumer, ProductMessageConsumer>();
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}
