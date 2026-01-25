using HomeAccounting.Users.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

class SpendingConfiguration: IEntityTypeConfiguration<Spending>
{
    public virtual void Configure(EntityTypeBuilder<Spending> builder)
    {
        builder.HasKey(spending => spending.Id);

        builder.Property(spending => spending.Id)
            .HasConversion(x => x.Value, x => new SpendingId(x))
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Ignore(spending => spending.Description);
        builder.Ignore(spending => spending.Sum);
        
        builder.Property(spending => spending.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));
        
        // builder.Property(spending => spending.BudgetId)
        //     .HasConversion(x => x.Value, x => new BudgetId(x));

        builder.Property<BudgetId>("BudgetId").HasColumnOrder(0).IsRequired();
    }
}