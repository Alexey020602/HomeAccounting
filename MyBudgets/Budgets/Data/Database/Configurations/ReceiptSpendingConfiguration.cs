using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Budgets.Data.Database.Configurations;

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
            });
        
        // builder.Navigation(r=>r.Products)
        //     .HasField("products")
        //     .UsePropertyAccessMode(PropertyAccessMode.Field)
        //     .AutoInclude();

        builder.Property(s => s.Sum);
    }
}