using BudgetData.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BudgetData;

public static class BudgetModule
{
    public static void AddBudgetServices(this IServiceCollection services)
    {
        services.TryAddScoped<ITransactionService, TransactionService>();
    }
}