using Microsoft.EntityFrameworkCore;

namespace MyBudgets.Budgets.Data.Database;

sealed class BudgetsContext(DbContextOptions<BudgetsContext> options) : DbContext(options)
{
    public const string ShemaName = "budgets";

    public DbSet<Spending> Spendings { get; set; }
    public DbSet<ReceiptSpending> ReceiptSpendings { get; set; }
    public DbSet<ManualSpending> ManualSpendings { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetRole> BudgetRoles { get; set; }
    public DbSet<BudgetUser> BudgetUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(ShemaName);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetsContext).Assembly);

        // modelBuilder.ApplyConfiguration(new SpendingConfiguration<Spending>());
        // modelBuilder.ApplyConfiguration(new SpendingConfiguration<ManualSpending>());
        // modelBuilder.ApplyConfiguration(new ReceiptSpendingConfiguration());
    }
}