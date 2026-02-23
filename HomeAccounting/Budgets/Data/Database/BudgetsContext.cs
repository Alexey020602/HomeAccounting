using HomeAccounting.Budgets.Data.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Categories.Data.DataBase.Configurations;
using HomeAccounting.Common.Model.ValueObjects;

namespace HomeAccounting.Budgets.Data.Database;

sealed class BudgetsContext(DbContextOptions<BudgetsContext> options) : DbContext(options)
{
    public const string ShemaName = "budgets";

    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetRole> BudgetRoles { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<ReceiptProcessingOutboxEntry> ReceiptProcessingOutbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(ShemaName);

        modelBuilder.ApplyConfiguration(new OperationConfiguration());
        modelBuilder.ApplyConfiguration(new ReceiptConfiguration());
        modelBuilder.ApplyConfiguration(new ReceiptProcessingOutboxEntryConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetRoleConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetUserConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Money>()
            .HaveConversion<MoneyConverter>();
    }
}