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
        
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(64);
        
        builder.HasIndex(x => x.Token).IsUnique();
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));
        
        builder.Property(x=>x.JwtId)
            .HasConversion(x => x.Value, x => new JwtId(x));
        
        builder.Property(x=>x.SessionId)
            .HasConversion(x => x.Value, x => new SessionId(x));
    }
}