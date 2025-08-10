using Budgets.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public class BudgetsContext(DbContextOptions<BudgetsContext> dbContextOptions): DbContext(dbContextOptions)
{
    internal DbSet<Budget> Budgets { get; set; }
    internal DbSet<Invitation> Invitations { get; set; }
    internal DbSet<BudgetUser> BudgetUsers { get; set; }
    internal DbSet<BudgetRole> BudgetRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(BudgetsDbConstants.SchemaName);
    }
}