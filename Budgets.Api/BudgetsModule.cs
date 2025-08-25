using Budgets.Core;
using Budgets.Core.CreateBudget;
using Budgets.Core.DeleteBudget;
using Budgets.Core.EditBudget;
using Budgets.Core.GetBudgetDetail;
using Budgets.Core.GetBudgets;
using Budgets.Core.GetBudgetUsers;
using Budgets.Core.UserInBudgetPermissions;
using Budgets.DataBase;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure;

namespace Budgets.Api;

public static class BudgetsModule
{
    public static void AddBudgetsModule(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder
            .AddDbContext<BudgetsContext>(
                databaseServiceName,
                optionsAction: options => options.SetUpBudgetsForDevelopment(),
                npgsqlOptionsAction: options => options
                    .MigrationsHistoryTable(DbConstants.MigrationTableName, BudgetsDbConstants.SchemaName)
            );

        builder.Services
            .AddScoped<IGetBudgetsService, GetBudgetsService>()
            .AddScoped<ICreateBudgetService, CreateBudgetService>()
            .AddScoped<IGetBudgetDetailService, GetBudgetDetailService>()
            .AddScoped<IAuthorizationHandler, BudgetRequirementsAuthorizationHandler>()
            .AddScoped<IUserBudgetPermissionsService, UserBudgetPermissionsService>()
            .AddScoped<IUpdateBudgetService,  UpdateBudgetService>()
            .AddScoped<IDeleteBudgetService,  DeleteBudgetService>()
            .AddScoped<IGetBudgetUsersService, GetBudgetUsersService>()
            .AddPipelineBehaviors()
            ;
    }

    private static IServiceCollection AddPipelineBehaviors(this IServiceCollection services) => services
        // .AddScoped(typeof(IPipelineBehavior<,IMaybe>), typeof(BudgetUsersPipelineBehavior<>))
        .AddScoped(typeof(IPipelineBehavior<,>), typeof(BudgetUsersResultedPipelineBehavior<,>));
}