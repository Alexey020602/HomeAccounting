using Checks.DataBase;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Checks.Tests;

public class TestDevelopmentChecksSeeding : IDisposable
{
    private readonly SqliteConnection conntection;

    private DbContextOptions Options => new DbContextOptionsBuilder()
        // .UseInMemoryDatabase("ChecksTest")
        .UseSqlite(conntection)
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
        .SetupChecksForDevelopment()
        .Options;

    private ChecksContext Context => new(new DbContextOptions<ChecksContext>(Options.Extensions
        .ToDictionary(
            x => x.GetType(),
            x => x
        )));

    public TestDevelopmentChecksSeeding()
    {
        conntection = new SqliteConnection("DataSource=:memory:");
        conntection.Open();
    }

    [Fact]
    public async Task TestSeedAsync()
    {
        await using var context = Context;

        await context.Database.MigrateAsync();
    }

    public void Dispose() => conntection.Dispose();
}