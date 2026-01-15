using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Budgets.Data.Database;

internal sealed class CategoryConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(c => c.Id)
            .HasConversion(x => x.Value, x => new CategoryId(x))
            .UseHiLo("CategoriesSequence");

        builder.Property(c => c.ParentCategoryId)
            .HasConversion(x => x!.Value.Value, x => new CategoryId(x));
    }
}