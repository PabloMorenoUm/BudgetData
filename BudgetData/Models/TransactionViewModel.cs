namespace BudgetData.Models;

public class TransactionViewModel
{
    public List<Transaction> Transactions { get; set; }
    public decimal TotalSum { get; set; }
}