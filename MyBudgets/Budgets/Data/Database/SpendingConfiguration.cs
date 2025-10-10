using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Budgets.Data.Database;

class SpendingConfiguration<TSpending>: IEntityTypeConfiguration<TSpending> where TSpending : Spending
{
    public virtual void Configure(EntityTypeBuilder<TSpending> builder)
    {
        builder.HasKey(spending => spending.Id);

        builder.Ignore(spending => spending.Description);
        builder.Ignore(spending => spending.Sum);

        builder.Property(spending => spending.Id)
            .HasConversion(x => x.Value, x => new SpendingId(x));
    }
}