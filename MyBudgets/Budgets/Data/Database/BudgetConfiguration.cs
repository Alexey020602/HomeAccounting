using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Budgets.Data.Database;

sealed class BudgetConfiguration: IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new BudgetId(x))
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.CreatorId)
            .HasConversion(x => x.Value, x => new(x));
        
        builder.Property(x=>x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}

sealed class BudgetRoleConfiguration : IEntityTypeConfiguration<BudgetRole>
{
    public void Configure(EntityTypeBuilder<BudgetRole> builder)
    {
        
        builder.Property(x => x.Id)
            .HasConversion(x=> x.Value, x => new (x))
            .UseHiLo("BudgetRoleSequence", BudgetsContext.ShemaName);
    }
}

sealed class BudgetUserConfiguration : IEntityTypeConfiguration<BudgetUser>
{
    public void Configure(EntityTypeBuilder<BudgetUser> builder)
    {
        builder.HasKey(nameof(BudgetUser.UserId), "BudgetId");
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new(x));
    }
}