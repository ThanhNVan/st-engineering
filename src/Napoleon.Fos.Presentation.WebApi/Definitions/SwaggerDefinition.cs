using Microsoft.OpenApi.Models;

namespace Napoleon.Fos.Presentation.WebApi.Definitions;

public static class SwaggerDefinition
{
    public static void AddSwaggerDefinition(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with JWT Token",
            });
            //options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            //{
            //    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            //    //{
            //    //    new OpenApiSecurityScheme
            //    //    {
            //    //        Reference = new OpenApiReference
            //    //        {
            //    //            Id = "Bearer",
            //    //            Type = ReferenceType.SecurityScheme
            //    //        }
            //    //    },
            //    //    new List<string>()
            //    //}
            //});

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }
}
