using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Budgets.Data.Database;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.Property(p => p.Id)
            .HasConversion(x => x.Value, x => new ProductId(x))
            .UseHiLo("ProductsSequence");

        builder.Property(p => p.CategoryId)
            .HasConversion(x => x!.Value.Value, x => new CategoryId(x));
    }
}