using System.Diagnostics;
using HomeAccounting.Users.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HomeAccounting.Users.TokensCleanup;

internal sealed partial class TokensCleanupWorker(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<TokensCleanupOptions> options,
    ActivitySource activitySource,
    ILogger<TokensCleanupWorker> logger) : BackgroundService
{
    private const int BatchSize = 50_000;
    private const int AfterExpirationLifePeriodInDays = 30;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dayInterval = options.Value.DayInterval;
        LogTokensCleanupWorkerStartedIntervalDayintervalDayS(logger, dayInterval);

        using var timer = new PeriodicTimer(TimeSpan.FromDays(dayInterval));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CleanupTokens(stoppingToken);
        }

        LogTokensCleanupWorkerStopped(logger);
    }

    private async Task CleanupTokens(CancellationToken stoppingToken)
    {
        using var activity = activitySource.StartActivity();

        try
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var usersContext = scope.ServiceProvider.GetRequiredService<UsersContext>();

            var cutoff = DateTimeOffset.UtcNow.AddDays(-AfterExpirationLifePeriodInDays);
            LogStartingTokensCleanupCutoffCutoffOTokensExpiredBeforeThisWillBeDeleted(logger, cutoff);

            var stopwatch = Stopwatch.StartNew();
            var totalDeleted = 0;
            var batchCount = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                var deleted = await usersContext.RefreshTokens
                    .Where(rt => rt.ExpiresAt < cutoff)
                    .OrderBy(rt => rt.Id)
                    .Take(BatchSize)
                    .ExecuteDeleteAsync(stoppingToken);

                totalDeleted += deleted;
                batchCount++;

                if (deleted > 0)
                {
                    LogTokensCleanupBatchBatchDeletedDeletedRowS(logger, batchCount, deleted);
                }

                if (deleted == 0)
                    break;

                await Task.Delay(TimeSpan.FromMilliseconds(200), stoppingToken);
            }

            stopwatch.Stop();

            LogTokensCleanupCompletedTotalDeletedTotaldeletedBatchesBatchcountDuration(logger, totalDeleted, batchCount, stopwatch.ElapsedMilliseconds);

            activity?.SetTag("tokens_cleanup.deleted_count", totalDeleted);
            activity?.SetTag("tokens_cleanup.batch_count", batchCount);
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            activity?.SetStatus(ActivityStatusCode.Ok);
            LogTokensCleanupWorkerIsStopping(logger);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            LogTokensCleanupFailedMessage(logger, ex.Message, ex);
        }
    }

    [LoggerMessage(LogLevel.Information, "Tokens cleanup worker started. Interval: {dayInterval} day(s)")]
    static partial void LogTokensCleanupWorkerStartedIntervalDayintervalDayS(ILogger<TokensCleanupWorker> logger, int dayInterval);

    [LoggerMessage(LogLevel.Information, "Tokens cleanup worker stopped")]
    static partial void LogTokensCleanupWorkerStopped(ILogger<TokensCleanupWorker> logger);

    [LoggerMessage(LogLevel.Debug, "Starting tokens cleanup. Cutoff: {cutoff:O} (tokens expired before this will be deleted)")]
    static partial void LogStartingTokensCleanupCutoffCutoffOTokensExpiredBeforeThisWillBeDeleted(ILogger<TokensCleanupWorker> logger, DateTimeOffset cutoff);

    [LoggerMessage(LogLevel.Debug, "Tokens cleanup batch {batch}: deleted {deleted} row(s)")]
    static partial void LogTokensCleanupBatchBatchDeletedDeletedRowS(ILogger<TokensCleanupWorker> logger, int batch, int deleted);

    [LoggerMessage(LogLevel.Information, "Tokens cleanup completed. Total deleted: {totalDeleted}, batches: {batchCount}, duration: {elapsedMs} ms")]
    static partial void LogTokensCleanupCompletedTotalDeletedTotaldeletedBatchesBatchcountDuration(ILogger<TokensCleanupWorker> logger, int totalDeleted, int batchCount, long elapsedMs);

    [LoggerMessage(LogLevel.Debug, "Tokens cleanup worker is stopping")]
    static partial void LogTokensCleanupWorkerIsStopping(ILogger<TokensCleanupWorker> logger);
    
    [LoggerMessage(LogLevel.Error, "Tokens cleanup failed: {Message}")]
    static partial void LogTokensCleanupFailedMessage(ILogger<TokensCleanupWorker> logger, string message, Exception ex);
}
