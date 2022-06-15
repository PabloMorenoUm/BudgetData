using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Models;

public class TransactionViewModel
{
    public Transaction Transaction { get; set; }
    public SelectList BudgetsSl { get; set; }
}