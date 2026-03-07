using HomeAccounting.Common.Infrastructure.Database;

namespace HomeAccounting.Users.Data.Database;

static class DatabaseModule
{
    public static void AddDatabase(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDbContext<UsersContext>(
            databaseServiceName,
            optionsAction: options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.SetUpAuthorizationForDevelopment();
                }
                else
                {
                    options.SetUpAuthorization();
                }
            },
            npgsqlOptionsAction: options => options.MigrationsHistoryTable(DbConstants.MigrationTableName, AuthorizationDbConstants.SchemaName)
        );
    }
}