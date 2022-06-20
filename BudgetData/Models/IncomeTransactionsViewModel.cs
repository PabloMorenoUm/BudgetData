using System.ComponentModel.DataAnnotations;

namespace BudgetData.Models;

public class IncomeTransactionsViewModel
{
    public List<Budget> BudgetList { get; set; }
}