using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Napoleon.Fos.Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    public static void ApplyEntityConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
