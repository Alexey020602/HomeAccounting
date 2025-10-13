using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBudgets.Users.Data.Database;

sealed class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne<RefreshToken>(u => u.RefreshToken);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x))
            .HasDefaultValueSql("gen_random_uuid()");
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(256);
        builder.Property(u => u.UserName).IsRequired();
        builder.Property(u => u.NormalizedUserName).IsRequired();
    }
}