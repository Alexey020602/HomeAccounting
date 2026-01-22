using Microsoft.EntityFrameworkCore;

namespace MyBudgets.Common.Infrastructure.Database;

static class AutomaticMigrationsExtension
{
    public static async Task MigrateDatabaseAsync<TContext>(this IHost host, CancellationToken cancellationToken = default) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        
        await context.Database.MigrateAsync(cancellationToken: cancellationToken);
    }
}