using ClientServerShared.Model.Money;
using HomeAccounting.Budgets.Data.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Categories.Data.DataBase.Configurations;

namespace HomeAccounting.Budgets.Data.Database;

sealed class BudgetsContext(DbContextOptions<BudgetsContext> options) : DbContext(options)
{
    public const string ShemaName = "budgets";

    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetRole> BudgetRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(ShemaName);

        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetsContext).Assembly);

        modelBuilder.ApplyConfiguration(new SpendingConfiguration());
        modelBuilder.ApplyConfiguration(new ReceiptSpendingConfiguration());
        modelBuilder.ApplyConfiguration(new ManualSpendingConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetRoleConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetUserConfiguration());
        // modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Money>()
            .HaveConversion<MoneyConverter>();
    }
}