using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Models;

public class TransactionViewModel
{
    public Transaction Transaction { get; set; }
    
    public SelectList? Budgets { get; set; }
}