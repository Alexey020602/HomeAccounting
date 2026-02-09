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

        builder.OwnsOne(
            r => r.FiscalData,
            fiscalData =>
            {
                fiscalData.Property(d => d.Fd).HasColumnName(nameof(ReceiptFiscalData.Fd)).IsRequired();
                fiscalData.Property(d => d.Fp).HasColumnName(nameof(ReceiptFiscalData.Fp)).IsRequired();
                fiscalData.Property(d => d.Fn).HasColumnName(nameof(ReceiptFiscalData.Fn)).IsRequired();
                fiscalData.Property(d=>d.PurchaseDate).HasColumnName(nameof(ReceiptFiscalData.PurchaseDate)).IsRequired();
                fiscalData.Property(d=>d.Sum).HasColumnName(nameof(ReceiptFiscalData.Sum)).IsRequired();
                fiscalData.HasIndex(d => new { d.Fd, d.Fn, d.Fp });
            });

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
