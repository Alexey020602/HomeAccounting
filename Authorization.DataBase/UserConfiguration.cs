using Authorization.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.DataBase;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne<RefreshToken>(u => u.RefreshToken);
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(256);
        builder.Property(u => u.UserName).IsRequired();
        builder.Property(u => u.NormalizedUserName).IsRequired();
    }
}