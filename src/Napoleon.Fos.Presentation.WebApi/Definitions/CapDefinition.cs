namespace Napoleon.Fos.Presentation.WebApi.Definitions;
using Savorboard.CAP.InMemoryMessageQueue;

public static class CapDefinition
{
    public static void AddCapMessageQueue(this IServiceCollection services, IConfiguration config)
    {
        // CAP
        services.AddCap(cap =>
        {
            // SQL Server as outbox/inbox store (transactional)
            cap.UseSqlServer(opt =>
            {
                opt.ConnectionString = config.GetConnectionString("Fos-DbConnection");
                opt.Schema = "cap";
            });

            // In-Memory queue (no broker needed — great for dev/testing)
            cap.UseInMemoryMessageQueue();

            // Optional: dashboard at /cap
            cap.UseDashboard(opt =>
            {
                opt.StatsPollingInterval = 5_000;
            });

            // Retry config
            cap.FailedRetryCount = 5;
            cap.FailedRetryInterval = 10; // seconds
        });
    }
}
