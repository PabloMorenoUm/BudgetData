using BudgetData.Data;
using BudgetData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Services;

public class TransactionService
{
    public const string BudgetCategoryAll = "Alle";

    private BudgetDataContext _context;
    private IQueryable<Transaction> _transactions;
    private IQueryable<string> _budgets;
    private readonly TransactionsTableViewModel _transactionsTableViewModel;

    public TransactionService(BudgetDataContext context)
    {
        _context = context;
        _transactions = from m in context.Transaction select m;
        _budgets = GetBudgetsFromTransactions();
        
        _transactionsTableViewModel = new TransactionsTableViewModel
        {
            TotalSum = 0,
            TransactionsPerCategories = new List<TransactionsPerCategory>(),
            Budgets = new SelectList(_budgets.ToList().Append(BudgetCategoryAll).OrderBy(i => i))
        };
    }

    public void SearchTransactionsByDescription(string? searchString)
    {
        if (string.IsNullOrEmpty(searchString)) return;
        _transactions = _transactions.Where(transaction =>
            transaction.DescriptionOfTransaction.Contains(searchString));
        _budgets = GetBudgetsFromTransactions();
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
            _transactionsTableViewModel.TransactionsPerCategories.Add(transactionsPerCategory);
            _transactionsTableViewModel.TotalSum += totalSum;
        }

        return _transactionsTableViewModel;
    }

    private IQueryable<string> GetBudgetsFromTransactions()
    {
        return _transactions.Select(transaction => transaction.Budget).Distinct();
    }

    public void BookIncomeTransactions(IncomeTransactionsViewModel incomeTransactions)
    {
        _context.AddRange(incomeTransactions.TransactionList);
        _context.SaveChanges();
    }
}