using HomeAccounting.Common.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.Data.DataBase;

internal static class DatabaseModule
{
    extension(IHostApplicationBuilder builder)
    {
        public void AddDatabase(string databaseServiceName)
        {
            builder.AddDbContext<CategoriesContext>(
                databaseServiceName,
                optionsAction: options =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        options.EnableSensitiveDataLogging();
                    }
                    else
                    {
                        
                    }
                },
                npgsqlOptionsAction: options =>
                {
                    options.MigrationsHistoryTable(DbConstants.MigrationTableName, CategoriesContext.Schema);
                }
                );
        }
    }
}