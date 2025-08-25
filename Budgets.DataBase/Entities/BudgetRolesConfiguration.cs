using Budgets.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgets.DataBase.Entities;

internal sealed class BudgetRolesConfiguration : IEntityTypeConfiguration<BudgetRole>
{
    public void Configure(EntityTypeBuilder<BudgetRole> builder)
    {
        builder.HasIndex(role => role.Name).IsUnique();
    }
}