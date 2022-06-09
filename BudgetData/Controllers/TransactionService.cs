using BudgetData.Data;
using BudgetData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetData.Controllers;

public class TransactionService
{
    private const string BudgetCategoryAll = "Alle";
    
    private IQueryable<Transaction> _transactions;
    public TransactionsTableViewModel TransactionsTableViewModel { get; }

    public TransactionService(BudgetDataContext context)
    {
        _transactions = from m in context.Transaction select m;
        var allBudgets = _transactions.Select(transaction => transaction.Budget).Distinct();
        TransactionsTableViewModel = new TransactionsTableViewModel
        {
            TotalSum = 0,
            TransactionsPerCategories = new List<TransactionsPerCategory>(),
            Budgets = new SelectList(allBudgets.ToList().Append(BudgetCategoryAll).OrderBy(i => i))
        };
    }

    public void SearchTransactionsByDescription(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            _transactions = _transactions.Where(transaction =>
                transaction.DescriptionOfTransaction.Contains(searchString));
        }
    }

    public IQueryable<string> FilterTransactionsByBudgets(string budgetFilter)
    {
        var filteredBudgets = _transactions.Select(transaction => transaction.Budget).Distinct();
            
        if (!string.IsNullOrEmpty(budgetFilter) && budgetFilter != BudgetCategoryAll)
        {
            filteredBudgets = filteredBudgets.Where(budget => budget == budgetFilter);
        }

        return filteredBudgets;
    }

    public void GenerateTablesPerBudget(IQueryable<string> filteredBudgets)
    {
        foreach (var budget in filteredBudgets)
        {
            var transactions = _transactions.Where(transactions => transactions.Budget == budget);
            var totalSum = transactions.Select(t => t.ValueOfTransaction).Sum();
            var transactionsPerCategory = new TransactionsPerCategory
            {
                Transactions = transactions.ToList(),
                TotalSum = totalSum
            };
            TransactionsTableViewModel.TransactionsPerCategories.Add(transactionsPerCategory);
            TransactionsTableViewModel.TotalSum += totalSum;
        }
    }
}