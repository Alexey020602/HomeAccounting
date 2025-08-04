using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Shared.Infrastructure;

public static class Extensions
{
    public static void AddDbContext<TContext>(this IHostApplicationBuilder builder, string serviceName, Action<DbContextOptionsBuilder>? optionsAction = null, Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null)
        where TContext: DbContext
    {
        
        builder.AddNpgsqlDbContext<TContext>(serviceName, configureDbContextOptions: options =>
        {
            optionsAction?.Invoke(options);
            
            npgsqlOptionsAction?.Invoke(new NpgsqlDbContextOptionsBuilder(options));
        });
    }
}