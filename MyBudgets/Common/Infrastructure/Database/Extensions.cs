using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace MyBudgets.Common.Infrastructure.Database;

public static class Extensions
{
    public static void AddDbContext<TContext>(this IHostApplicationBuilder builder, string serviceName, Action<DbContextOptionsBuilder>? optionsAction = null, Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null)
        where TContext: DbContext
    {
        
        builder.AddNpgsqlDbContext<TContext>(serviceName, configureDbContextOptions: options =>
        {
            optionsAction?.Invoke(options);

            options.UseExceptionProcessor();
            
            npgsqlOptionsAction?.Invoke(new NpgsqlDbContextOptionsBuilder(options));
        });
    }
}