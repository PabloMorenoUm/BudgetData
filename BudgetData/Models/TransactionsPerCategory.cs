namespace BudgetData.Models;

public class TransactionsPerCategory
{
    public List<Transaction> Transactions { get; set; }
    public decimal TotalSum { get; set; }
}