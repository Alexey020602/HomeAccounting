using Authorization;
using Authorization.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Receipts.DataBase;

namespace Shared.Infrastructure;

public static class Extensions
{
    private static void AddDbContext<TContext>(this IHostApplicationBuilder builder, string serviceName, Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext: DbContext
    {
        builder.AddNpgsqlDbContext<TContext>(serviceName, configureDbContextOptions: optionsAction);
    }

    public static void AddDbContexts(this IHostApplicationBuilder builder)
    {
        builder.AddDbContext<ReceiptsContext>("HomeAccounting", options => options.SetupChecksForDevelopment());
        builder.AddDbContext<AuthorizationContext>("HomeAccounting", options => options.SetUpAuthorizationForDevelopment());
    }
}