using BudgetData.Data;
using BudgetData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Services;

public class BudgetService
{
    private BudgetDataContext _context;
    private IQueryable<Budget> _budgets;

    public BudgetService(BudgetDataContext context)
    {
        _context = context;
        _budgets = from b in context.Budget select b;
    }

    public SelectList CreateBudgetsFromDB()
    {
        return new SelectList(_budgets.Select(budget => budget.Purpose).ToList());
    }
}