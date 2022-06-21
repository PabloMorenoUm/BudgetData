using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Models;

public class CreateTransactionViewModel
{
    public Transaction? Transaction { get; set; }
    
    public SelectList? Budgets { get; set; }
}