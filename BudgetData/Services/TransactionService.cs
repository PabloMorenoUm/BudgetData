using BudgetData.Data;
using BudgetData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Services;

public class TransactionService
{
    public const string BudgetCategoryAll = "Alle";

    private BudgetDataContext _context;
    private IQueryable<Transaction> _transactions;
    private IQueryable<string?> _budgets;
    private readonly TransactionsTableViewModel _transactionsTableViewModel;

    public TransactionService(BudgetDataContext context)
    {
        _context = context;
        _transactions = from m in context.Transaction select m;
        _budgets = GetBudgetsWithTransactionsFromDb();
        var budgetListWithAll = _budgets.ToList();
        budgetListWithAll.Insert(0, BudgetCategoryAll);

        _transactionsTableViewModel = new TransactionsTableViewModel
        {
            TotalSum = 0,
            TransactionsPerCategories = new List<TransactionsPerCategory>(),
            Budgets = new SelectList(budgetListWithAll)
        };
    }

    public void SearchTransactionsByDescription(string? searchString)
    {
        if (string.IsNullOrEmpty(searchString)) return;
        _transactions = _transactions.Where(transaction =>
            transaction.DescriptionOfTransaction!.Contains(searchString));
        _budgets = GetBudgetsWithTransactionsFromDb();
    }

    public void FilterBudgets(string? budgetFilter)
    {
        if (string.IsNullOrEmpty(budgetFilter) || budgetFilter == BudgetCategoryAll) return;
        _budgets = _budgets.Where(budget => budget == budgetFilter);
    }

    public TransactionsTableViewModel GenerateTablesPerBudget()
    {
        foreach (var budget in _budgets)
        {
            var transactions = _transactions.Where(transactions => transactions.Budget == budget)
                .OrderByDescending(t => t.DateOfTransaction).ToList();
            var totalSum = transactions.Select(t => t.ValueOfTransaction).Sum();
            if (_budgets.Count() > 1)
            {
                transactions = transactions.Take(5).ToList();
            }

            var transactionsPerCategory = new TransactionsPerCategory
            {
                Transactions = transactions,
                TotalSum = totalSum
            };
            if (_transactionsTableViewModel.TransactionsPerCategories != null)
                _transactionsTableViewModel.TransactionsPerCategories.Add(transactionsPerCategory);
            _transactionsTableViewModel.TotalSum += totalSum;
        }

        return _transactionsTableViewModel;
    }

    public void BookIncomeTransactionsFromDict(Dictionary<string, decimal> incomeValues)
    {
        foreach (var budget in incomeValues.Keys)
        {
            _context.Add(createIncomeTransaction(budget, incomeValues[budget]));
        }
        _context.SaveChanges();
    }

    public IQueryable<string?> GetBudgetsWithTransactionsFromDb()
    {
        var transactions = _transactions.Select(transaction => transaction.Budget).Distinct();
        var query = from b in _context.Budget select b;
        return query.OrderBy(b => b.Priority)
            .Select(b => b.Purpose)
            .Where(b => transactions.Contains(b));
    }

    private Transaction createIncomeTransaction(string budget, decimal value)
    {
        return new Transaction()
        {
            Budget = budget,
            DateOfTransaction = DateTime.Today,
            DescriptionOfTransaction = "Gehalt",
            ValueOfTransaction = value
        };
    }
}