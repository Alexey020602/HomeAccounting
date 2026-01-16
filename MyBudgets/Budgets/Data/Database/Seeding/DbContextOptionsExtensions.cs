using Microsoft.EntityFrameworkCore;

namespace MyBudgets.Budgets.Data.Database.Seeding;

public static class DbContextOptionsExtensions
{
    extension(DbContextOptionsBuilder builder)
    {
        public DbContextOptionsBuilder SetUpBudgets() => 
            builder
            .UseSeeding(Seed)
            .UseAsyncSeeding(SeedAsync);

        public DbContextOptionsBuilder SetUpBudgetsForDevelopment() => builder
            .UseSeeding(SeedForDevelopment)
            .UseAsyncSeeding(SeedForDevelopmentAsync);
    }

    private static void Seed(DbContext context, bool dbHasChanges)
    {
        if (context is not BudgetsContext budgetsContext) return;

        budgetsContext.AddBudgetRoles();

        budgetsContext.SaveChanges();
    }
    
    private static async Task SeedAsync(DbContext context, bool dbHasChanges, CancellationToken cancellationToken)
    {
        if (context is not BudgetsContext budgetsContext) return;

        await budgetsContext.AddBudgetRolesAsync(cancellationToken);

        await budgetsContext.SaveChangesAsync(cancellationToken);
    }

    private static void SeedForDevelopment(DbContext context, bool dbHasChanges)
    {
        if (context is not BudgetsContext budgetsContext) return;

        budgetsContext.AddBudgetRoles();
        budgetsContext.AddBudgets();

        budgetsContext.SaveChanges();
    }

    private static async Task SeedForDevelopmentAsync(DbContext context, bool dbHasChanges,
        CancellationToken cancellationToken)
    {
        if (context is not BudgetsContext budgetsContext) return;
        
        await budgetsContext.AddBudgetRolesAsync(cancellationToken);

        await budgetsContext.AddBudgetsAsync(cancellationToken);
        
        await budgetsContext.SaveChangesAsync(cancellationToken);
    }
}