using BudgetData.Data;
using BudgetData.Models;

namespace BudgetData.Services;

public class KassensturzService
{
    private TransactionService _transactionService;

    public KassensturzService(BudgetDataContext context)
    {
        _transactionService = new TransactionService(context);
    }

    public decimal CalculateDifference(Kassensturz kassensturz)
    {
        var gesamtbetrag = _transactionService.GenerateTablesPerBudget().TotalSum;
        return gesamtbetrag - kassensturz.Sum;
    }
    
    
}