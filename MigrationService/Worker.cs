using System.Diagnostics;
using Checks.DataBase;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace MigrationService;

public class Worker(
    IServiceProvider services,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger
) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var applicationContextSeed = scope.ServiceProvider.GetRequiredService<ApplicationContextSeed>();
            logger.LogInformation("Prepare database");
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Database migration completed");
            logger.LogInformation("База данных готова к работе");
            await applicationContextSeed.Seed(cancellationToken);
            logger.LogInformation("Data added to database");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }
}