using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Shared.Infrastructure;

public static class Extensions
{
    private static void AddDbContext<TContext>(this IHostApplicationBuilder builder, string serviceName, Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext: DbContext
    {
        builder.AddNpgsqlDbContext<TContext>(serviceName, configureDbContextOptions: optionsAction);
    }
}