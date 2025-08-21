using Authorization.Contracts;
using Budgets.Core.GetBudgetDetail;
using Budgets.Core.Model;
using Budgets.Core.UserInBudgetPermissions;
using Budgets.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public static class DbContextOptionsExtensions
{
    public static DbContextOptionsBuilder SetUpBudgets(this DbContextOptionsBuilder builder) => builder;

    public static DbContextOptionsBuilder SetUpBudgetsForDevelopment(this DbContextOptionsBuilder builder) => builder
        .SetUpBudgets()
        .UseSeeding(Seed)
        .UseAsyncSeeding(SeedAsync);

    private static void Seed(DbContext context, bool dbHasChanges)
    {
        if (context is not BudgetsContext budgetsContext) return;

        foreach (var budget in CreateDefaultBudgets())
        {
            if (budgetsContext.Budgets.SingleOrDefault(b => b.Id == budget.Id) is not null)
                continue;

            budgetsContext.Budgets.Add(budget);
        }

        foreach (var budgetRole in CreateDefaultBudgetRoles())
        {
            if (budgetsContext.BudgetRoles.SingleOrDefault(b => b.Id == budgetRole.Id) is not null)
                continue;

            budgetsContext.BudgetRoles.Add(budgetRole);
        }

        foreach (var budgetUser in CreateDefaultBudgetUsers())
        {
            if (budgetsContext.BudgetUsers.SingleOrDefault(u =>
                    u.BudgetId == budgetUser.BudgetId
                    && u.UserId == budgetUser.UserId
                    && u.BudgetRoleId == budgetUser.BudgetRoleId
                ) is not null)
                continue;

            budgetsContext.BudgetUsers.Add(budgetUser);
        }

        budgetsContext.SaveChanges();
    }

    private static async Task SeedAsync(DbContext context, bool dbHasChanges, CancellationToken cancellationToken)
    {
        if (context is not BudgetsContext budgetsContext) return;

        foreach (var budget in CreateDefaultBudgets())
        {
            if (await budgetsContext.Budgets.SingleOrDefaultAsync(b => b.Id == budget.Id,
                    cancellationToken: cancellationToken) is not null)
                continue;

            budgetsContext.Budgets.Add(budget);
        }

        foreach (var budgetRole in CreateDefaultBudgetRoles())
        {
            if (await budgetsContext.BudgetRoles.SingleOrDefaultAsync(r => r.Id == budgetRole.Id,
                    cancellationToken: cancellationToken) is not null)
                continue;

            budgetsContext.BudgetRoles.Add(budgetRole);
        }

        foreach (var budgetUser in CreateDefaultBudgetUsers())
        {
            if (await budgetsContext.BudgetUsers.SingleOrDefaultAsync(
                    u =>
                        u.BudgetId == budgetUser.BudgetId
                        && u.UserId == budgetUser.UserId
                        && u.BudgetRoleId == budgetUser.BudgetRoleId,
                    cancellationToken: cancellationToken
                ) is not null)
                continue;

            budgetsContext.BudgetUsers.Add(budgetUser);
        }

        await budgetsContext.SaveChangesAsync(cancellationToken);
    }

    private static List<Budget> CreateDefaultBudgets() =>
    [
        new()
        {
            Id = BudgetsSeedingConstants.FirstBudgetId,
            BeginOfPeriod = 7,
            CreatorId = UserConstants.DefaultUserId,
            CreationDate = new DateTime(2025, 8, 11, 15, 32, 14, 23, DateTimeKind.Utc),
            Name = "Мой и Сашин бюджет",
            Limit = 25000,
        },
        new()
        {
            Id = BudgetsSeedingConstants.SecondBudgetId,
            BeginOfPeriod = 1,
            CreatorId = UserConstants.DefaultUserId,
            CreationDate = new DateTime(2025, 7, 15, 8, 53, 22, 893, DateTimeKind.Utc),
            Name = "Личный бюджет"
        }
    ];

    private static List<BudgetRole> CreateDefaultBudgetRoles() =>
    [
        new()
        {
            Id = BudgetsSeedingConstants.UserBudgetRoleId,
            Name = BudgetRole.UserRoleName,
            Permissions = BudgetPermissions.Read
        },
        new()
        {
            Id = BudgetsSeedingConstants.AdminBudgetRoleId,
            Name = BudgetRole.AdminRoleName,
            Permissions = BudgetPermissions.Read | BudgetPermissions.Edit
        },
        new()
        {
            Id = BudgetsSeedingConstants.OwnerBudgetRoleId,
            Name = BudgetRole.OwnerRoleName,
            Permissions = BudgetPermissions.Read | BudgetPermissions.Edit | BudgetPermissions.Delete
        },
    ];

    private static List<BudgetUser> CreateDefaultBudgetUsers() =>
    [
        new()
        {
          BudgetId  = BudgetsSeedingConstants.FirstBudgetId,
          UserId = UserConstants.DefaultUserId,
          BudgetRoleId = BudgetsSeedingConstants.OwnerBudgetRoleId,
        },
        new()
        {   
            BudgetId  = BudgetsSeedingConstants.SecondBudgetId,
            UserId = UserConstants.DefaultUserId,
            BudgetRoleId = BudgetsSeedingConstants.OwnerBudgetRoleId,
        },
    ];
}

public static class BudgetsSeedingConstants
{
    public static readonly Guid FirstBudgetId = Guid.Parse("01989e7d-5251-759b-b91a-1b51403e8039");
    public static readonly Guid SecondBudgetId = Guid.Parse("01989e7d-8b91-75e8-91a1-7edc1f9b3385");
    public static readonly Guid UserBudgetRoleId = Guid.Parse("0198a727-9fcf-724a-b1bb-0595bf865d29");
    public static readonly Guid AdminBudgetRoleId = Guid.Parse("0198a728-70bd-78c4-85fc-2b44c20a5293");
    public static readonly Guid OwnerBudgetRoleId = Guid.Parse("0198a772-70d8-7739-bdd9-692a8b808d5a");
}