using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Budgets.Data.Database.Configurations;

sealed class ReceiptProcessingOutboxEntryConfiguration : IEntityTypeConfiguration<ReceiptProcessingOutboxEntry>
{
    public void Configure(EntityTypeBuilder<ReceiptProcessingOutboxEntry> builder)
    {
        builder.ToTable("ReceiptProcessingOutboxEntry");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.ReceiptId)
            .HasConversion(x => x.Value, x => new ReceiptId(x));

        builder.Property(e => e.LastErrorMessage).HasMaxLength(500);

        builder.HasIndex(e => e.ReceiptId).IsUnique();
        builder.HasIndex(e => new { e.Status, e.NextRetryAt });
    }
}
