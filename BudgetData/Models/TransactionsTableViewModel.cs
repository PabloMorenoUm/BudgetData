namespace BudgetData.Models;

public class TransactionsTableViewModel
{
    public List<TransactionsPerCategory> TransactionsPerCategories { get; set; }
    public decimal TotalSum { get; set; }
}