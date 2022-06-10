using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Models;

public class BudgetList
{
    private static List<string> _budgets = new()
    {
        "Miete",
        "Essen",
        "Freizeit",
        "Sonstiges"
    };

    public static SelectList BudgetsSL = new(_budgets);
}