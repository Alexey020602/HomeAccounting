using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

sealed class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(x => x.Value, x => new ReceiptId(x))
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(r => r.BudgetId)
            .HasConversion(x => x.Value, x => new BudgetId(x));

        builder.Property(r => r.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(r => r.LastErrorMessage).HasMaxLength(500);

        // Фискальные данные (Value Objects)
        builder.Property(r => r.Fn)
            .HasConversion(x => x.Value, x => FiscalNumber.Create(x))
            .HasColumnName("Fn")
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(r => r.Fd)
            .HasConversion(x => x.Value, x => FiscalDocument.Create(x))
            .HasColumnName("Fd")
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(r => r.Fp)
            .HasConversion(x => x.Value, x => FiscalSign.Create(x))
            .HasColumnName("Fp")
            .IsRequired()
            .HasMaxLength(10);

        builder.Ignore(r => r.CalculatedSum);

        // Индекс для проверки дубликатов
        builder.HasIndex(r => new { r.Fd, r.Fn, r.Fp });

        builder.OwnsMany(r => r.Products, b =>
        {
            b.HasKey(product => product.Id);
            b.Property(p => p.Id)
                .HasConversion(x => x.Value, x => new ProductId(x))
                .HasDefaultValueSql("gen_random_uuid()");
            b.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
            b.Property(p => p.CategoryId)
                .HasConversion(x => x!.Value.Value, x => new CategoryId(x));
            b.HasIndex(p => p.CategoryId);
        });
    }
}
