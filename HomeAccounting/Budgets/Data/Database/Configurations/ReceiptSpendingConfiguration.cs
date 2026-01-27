using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

sealed class ReceiptSpendingConfiguration: IEntityTypeConfiguration<ReceiptSpending>
{
    public void Configure(EntityTypeBuilder<ReceiptSpending> builder)
    {
        
        builder.OwnsOne(
            receipt => receipt.FiscalData,
            fiscalData =>
            {
                fiscalData.Property(d => d.Fd).HasColumnName(nameof(ReceiptFiscalData.Fd)).IsRequired();
                fiscalData.Property(d => d.Fp).HasColumnName(nameof(ReceiptFiscalData.Fp)).IsRequired();
                fiscalData.Property(d => d.Fn).HasColumnName(nameof(ReceiptFiscalData.Fn)).IsRequired();

                fiscalData.HasIndex(d => new { d.Fd, d.Fn, d.Fp }).IsUnique();
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

        builder.Property(r => r.LastErrorMessage).HasMaxLength(500);

        builder.OwnsMany(r => r.Attempts, b =>
        {
            b.HasKey(attempt => attempt.Id);

            b.Property(attempt => attempt.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            b.Property(attempt => attempt.ErrorMessage).HasMaxLength(500);
        });
    }
}