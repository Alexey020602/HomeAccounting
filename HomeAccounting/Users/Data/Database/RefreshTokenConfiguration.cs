using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Users.Data.Database;

internal sealed class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id)
            .HasConversion(x => x.Value, x => new RefreshTokenId(x))
            .HasDefaultValueSql("gen_random_uuid()");
        
        
    }
}