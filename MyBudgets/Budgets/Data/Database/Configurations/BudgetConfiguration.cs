using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Budgets.Data.Database.Configurations;

sealed class BudgetConfiguration: IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {

        builder.HasKey(b => b.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new BudgetId(x))
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.CreatorId)
            .HasConversion(x => x.Value, x => new(x));
        
        builder.Property(x=>x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        // builder.Ignore(x => x.Spendings);
        // builder.Ignore(x => x.BudgetUsers);

        builder.Navigation(b => b.Spendings)
            .HasField("spendings")
            ;
            
        builder.Navigation(b=>b.BudgetUsers)
            .HasField("budgetUsers")
            ;
        
        builder.HasMany(b=>b.Spendings)
            .WithOne()
            .HasForeignKey("BudgetId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            
            ;
        
        builder.HasMany(b=>b.BudgetUsers)
            .WithOne()
            .HasForeignKey("BudgetId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
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

        builder.Property<BudgetId>("BudgetId").HasColumnOrder(0).IsRequired();
        
        builder.Property(u => u.BudgetRoleId)
            .HasConversion(x => x.Value, x => new(x));
    }
}