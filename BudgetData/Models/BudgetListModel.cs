using System.ComponentModel.DataAnnotations;

namespace BudgetData.Models;

public class BudgetListModel
{
    public List<Budget>? BudgetList { get; set; }
}