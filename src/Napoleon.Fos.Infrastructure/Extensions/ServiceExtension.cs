using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Napoleon.Fos.Core.Repositories;
using Napoleon.Fos.Core.Settings;
using Napoleon.Fos.Core.UnitOfWork;
using Napoleon.Fos.Infrastructure.AppDbContexts;
using Napoleon.Fos.Infrastructure.Repositories;
using Napoleon.Fos.Infrastructure.UnitOfWork;



namespace Napoleon.Fos.Infrastructure.Extensions;

public static class ServiceExtension
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Fos-DbConnection");
        service.AddPooledDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.EnableRetryOnFailure();
                    sqlServerOptionsAction.MigrationsHistoryTable("__EFMigrationsHistory", "Migration");
                    sqlServerOptionsAction.MigrationsAssembly("Napoleon.Fos.Infrastructure");
                })
                .EnableServiceProviderCaching();
            options.EnableSensitiveDataLogging();
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        service.AddScoped(p => p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

        service.AddRepository();

        var jwtSettings = new JwtSettings();
        config.GetSection("JwtSettings").Bind(jwtSettings);
        service.AddSingleton(jwtSettings);

        //service.Configure<JwtSettings>(config.GetSection("JwtSettings"));

    }

    private static void AddRepository(this IServiceCollection service)
    {
        service.AddScoped<ICustomerRepository, CustomerRepository>();
        service.AddScoped<IAuthenticationTokenRepository, AuthenticationTokenRepository>();
        service.AddScoped<IProductRepository, ProductRepository>();
        //service.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        service.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
    }
}
