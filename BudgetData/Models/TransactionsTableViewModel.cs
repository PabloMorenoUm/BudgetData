using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Models;

public class TransactionsTableViewModel
{
    public List<TransactionsPerCategory> TransactionsPerCategories { get; set; }
    public string? BudgetFilter { get; set; }
    public SelectList? Budgets { get; set; }
    public decimal TotalSum { get; set; }
}