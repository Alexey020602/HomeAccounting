using Budgets.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public class BudgetsContext(DbContextOptions<BudgetsContext> dbContextOptions): DbContext(dbContextOptions)
{
    DbSet<Budget> Budgets { get; set; }
    DbSet<Invitation> Invitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(BudgetsDbConstants.SchemaName);
    }
}