using FluentValidation.AspNetCore;
using FluentValidation;
using Napoleon.Fos.Presentation.WebApi.Interceptor;
using System.Reflection;

namespace Napoleon.Fos.Presentation.WebApi.ServiceExtension;

public static class RegisterService
{
    public static void AddExceptionHandlerDefinition(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    public static void AddAutoValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("default", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
    }
}
