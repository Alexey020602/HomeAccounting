using System.Diagnostics;
using DataBase;
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
            // await dbContext.EnsureDatabaseAsync(cancellationToken);
            // await dbContext.RunMigrationAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
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