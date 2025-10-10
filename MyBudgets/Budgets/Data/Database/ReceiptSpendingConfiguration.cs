using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data.Database;

sealed class ReceiptSpendingConfiguration: SpendingConfiguration<ReceiptSpending>
{
    public override void Configure(EntityTypeBuilder<ReceiptSpending> builder)
    {
        base.Configure(builder);

        builder.Ignore(receiptSpending => receiptSpending.Products);
        
        builder.Property(receiptSpanding => receiptSpanding.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));
        
        builder.Property(receiptSpanding => receiptSpanding.BudgetId)
            .HasConversion(x => x.Value, x => new BudgetId(x));
        
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