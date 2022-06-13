namespace BudgetData.Models;

public class TransactionsPerCategory
{
    public List<Transaction> Transactions { get; set; }
    public decimal TotalSum { get; set; }

    public string GetBudget()
    {
        return Transactions[0].Budget ?? "none";
    }
}