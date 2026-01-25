using HomeAccounting.Categories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

internal sealed class ManualSpendingConfiguration: IEntityTypeConfiguration<ManualSpending>
{
    public void Configure(EntityTypeBuilder<ManualSpending> builder)
    {
        builder.Property(s => s.Description);

        builder.Property(s => s.Sum);

        builder.Property(s => s.CategoryId)
            .HasConversion(x => x!.Value.Value, x => new CategoryId(x));
    }
}