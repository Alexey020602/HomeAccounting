using Checks.Contracts;
using Checks.Core;
using Checks.DataBase;
using Shared.Model.Requests;

namespace MigrationService;

public class ApplicationContextSeed
{
    private readonly ICheckUseCase checkUseCase;
    private readonly ApplicationContext context;
    private readonly IHostEnvironment hostEnvironment;
    private readonly ILogger<ApplicationContextSeed> logger;
    private readonly IReadOnlyList<RawCheckRequest> requests;

    public ApplicationContextSeed(
        ICheckUseCase checkUseCase,
        ApplicationContext context,
        ILogger<ApplicationContextSeed> logger,
        IHostEnvironment hostEnvironment)
    {
        this.context = context;
        this.hostEnvironment = hostEnvironment;
        this.checkUseCase = checkUseCase;
        this.logger = logger;

        requests = new[]
            {
                "t=20250107T104157&s=1691.16&fn=7380440801290534&i=9446&fp=1880975916&n=1",
                "t=20250112T1715&s=93.00&fn=7381440800435707&i=21728&fp=3846028993&n=1",
                "t=20250113T2321&s=1015.73&fn=7284440500173838&i=62887&fp=182171056&n=1",
                "t=20250111T105907&s=1236.35&fn=7380440801290534&i=10277&fp=840967215&n=1",
                "t=20250104T1233&s=1289.00&fn=7380440700673345&i=26636&fp=1241320200&n=1"
            }
            .Select(raw => new RawCheckRequest
            {
                QrRaw = raw,
                AddedTime = DateTimeOffset.UtcNow
            })
            .ToList();
    }

    public async Task Seed(CancellationToken token = default)
    {
        try
        {
            await AddDefaultUser(token);
            // foreach (var request in requests) await checkUseCase.SaveCheck(request, User.Default);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Seeding error");
            throw;
        }
    }

    private Task AddDefaultUser(CancellationToken token = default)
    {
        if (context.Users.Any(user => user.Id == Checks.DataBase.Entities.User.Default.Id)) return Task.CompletedTask;
        
        context.Users.Add(Checks.DataBase.Entities.User.Default);
        
        return context.SaveChangesAsync(token);
    }
}