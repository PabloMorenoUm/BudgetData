using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Models;

public class BudgetList
{
    private List<Budget> _budgetList;

    public BudgetList(List<Budget> budgetList)
    {
        _budgetList = budgetList;
    }

    private List<string> GetBudgetNames()
    {
        return _budgetList.Select(budget => budget.Purpose).ToList();
    }

    private static List<string> _budgets = new()
    {
        "Miete",
        "Essen",
        "Freizeit",
        "Sonstiges"
    };

    public SelectList BudgetsSL()
    {
        return new SelectList(GetBudgetNames());
    }
}