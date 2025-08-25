using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shared.Tests;

public sealed class TestDatabaseContextFactory: IDisposable
{
    private readonly SqliteConnection connection;

    public TestDatabaseContextFactory()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
    }

    public DbContextOptions CreateContextOptions(Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        var builder = new DbContextOptionsBuilder()
            .UseSqlite(connection)
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        optionsAction?.Invoke(builder);
        return builder.Options;
    }

    private DbContextOptions<TContext> CreateContextOptions<TContext>(DbContextOptions options)
        where TContext : DbContext
        => new(options.Extensions.ToDictionary(
            x => x.GetType(),
            x => x
        ));
    public TContext CreateContext<TContext>(Action<DbContextOptionsBuilder>? optionsAction = null) where TContext : DbContext
    {
        return (TContext) (Activator.CreateInstance(typeof(TContext), CreateContextOptions<TContext>(CreateContextOptions(optionsAction))) 
                           ?? throw new InvalidOperationException());
    }
    public void Dispose() => connection.Dispose();
}