using HomeAccounting.Categories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.Property(p => p.Id)
            .HasConversion(x => x.Value, x => new ProductId(x))
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .HasConversion(x => x!.Value.Value, x => new CategoryId(x));
    }
}