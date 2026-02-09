using HomeAccounting.Categories.Data;
using HomeAccounting.Users.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

internal sealed class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("Operation");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(x => x.Value, x => new OperationId(x))
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(o => o.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Property<BudgetId>("BudgetId").HasColumnOrder(0).IsRequired();

        builder.Property(o => o.Description)
            .IsRequired();

        builder.Property(o => o.Sum);

        builder.Property(o => o.CategoryId)
            .HasConversion(x => x!.Value.Value, x => new CategoryId(x));
    }
}
