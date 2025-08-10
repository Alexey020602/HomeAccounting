using Budgets.DataBase;
using Microsoft.Extensions.DependencyInjection;

namespace Budgets.Api;

public static class AccountingModule
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services) =>
        services
            .AddDbContext<BudgetsContext>();
}