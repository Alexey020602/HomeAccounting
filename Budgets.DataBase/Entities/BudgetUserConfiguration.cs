using Budgets.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgets.DataBase.Entities;

internal sealed class BudgetUserConfiguration : IEntityTypeConfiguration<BudgetUser>
{
    public void Configure(EntityTypeBuilder<BudgetUser> builder)
    {
        builder.HasKey(user => new { user.UserId, user.BudgetId, user.BudgetRoleId });
    }
}