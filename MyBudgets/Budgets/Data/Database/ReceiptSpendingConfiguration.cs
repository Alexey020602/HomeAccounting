using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data.Database;

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
    }
}