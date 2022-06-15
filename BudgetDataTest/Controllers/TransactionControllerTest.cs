using System;
using System.Diagnostics;
using System.Linq;
using BudgetData.Controllers;
using BudgetData.Data;
using BudgetData.Models;
using BudgetData.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BudgetDataTest.Controllers;

public class TransactionControllerTest : IDisposable
{
    DbContextOptions<BudgetDataContext> Options { get; }
    TransactionsTableViewModel _table;

    public TransactionControllerTest()
    {
        Options = new DbContextOptionsBuilder<BudgetDataContext>()
            .UseInMemoryDatabase(databaseName: "TransactionControllerTestDatabase")
            .Options;

        using (var context = new BudgetDataContext(Options))
        {
            context.Transaction!.Add(new Transaction
            {
                DescriptionOfTransaction = "abc", Budget = "Budget1", ValueOfTransaction = (decimal)0.01
            });
            context.Transaction.Add(new Transaction
            {
                DescriptionOfTransaction = "bcd", Budget = "Budget1", ValueOfTransaction = (decimal)0.10
            });
            context.Transaction.Add(new Transaction
            {
                DescriptionOfTransaction = "cde", Budget = "Budget2", ValueOfTransaction = (decimal)1.00
            });
            context.Transaction.Add(new Transaction
            {
                DescriptionOfTransaction = "xyz", Budget = "Hifi", ValueOfTransaction = (decimal)10.00
            });
            context.SaveChanges();
        }
    }

    private void CreateViewDataModel(string? budgetFilter = null, string? searchString = null)
    {
        using (var context = new BudgetDataContext(Options))
        {
            var transactionController = new TransactionController(context);
            var view = transactionController.Index(budgetFilter, searchString) as ViewResult;
            _table = (TransactionsTableViewModel)view.ViewData.Model;
        }
    }

    [Fact]
    public void Index_ShouldCalculateTotalSumOfAllTransactions()
    {
        CreateViewDataModel();
        _table.TotalSum.Should().Be((decimal)11.11);
    }

    [Fact]
    public void Index_ShouldContainThreeDistinctBudgets()
    {
        CreateViewDataModel();
        _table.TransactionsPerCategories.Count.Should().Be(3);
    }

    [Fact]
    public void Index_ShouldCalculateSubtotalSumOfAnyBudget()
    {
        CreateViewDataModel();
        _table.TransactionsPerCategories.First(transactionList => transactionList.GetBudget() == "Budget1").TotalSum
            .Should().Be((decimal)0.11);
        _table.TransactionsPerCategories.First(transactionList => transactionList.GetBudget() == "Budget1").Transactions
            .Count.Should().Be(2);
        _table.TransactionsPerCategories.First(transactionList => transactionList.GetBudget() == "Budget2").TotalSum
            .Should().Be((decimal)1.00);
        _table.TransactionsPerCategories.First(transactionList => transactionList.GetBudget() == "Budget2").Transactions
            .Count.Should().Be(1);
    }

    [Fact]
    public void IndexWithFilter_ShouldContainOneBudget()
    {
        CreateViewDataModel("Budget1");
        _table.TransactionsPerCategories.Count.Should().Be(1);
        _table.TotalSum.Should().Be((decimal)0.11);
    }

    [Fact]
    public void IndexWithFilter_ShouldContainAllBudgets()
    {
        CreateViewDataModel("Alle");
        _table.TransactionsPerCategories.Count.Should().Be(3);
        _table.TotalSum.Should().Be((decimal)11.11);
    }

    [Fact]
    public void IndexWithSearch_ShouldContainMatchingTransactions()
    {
        CreateViewDataModel(searchString: "cd");
        _table.TransactionsPerCategories.Count.Should().Be(2);
        _table.TransactionsPerCategories.First(transactionList => transactionList.GetBudget() == "Budget1").Transactions
            .Count.Should().Be(1);
    }

    [Fact]
    public void IndexWithFilterAndSearch_ShouldContainMatchingTransactions()
    {
        CreateViewDataModel("Budget1", "cd");
        _table.TransactionsPerCategories.Count.Should().Be(1);
    }

    [Fact]
    public void Create_ShouldExcludeCategoryAll()
    {
        const string budgetName = TransactionService.BudgetCategoryAll;

        using (var context = new BudgetDataContext(Options))
        {
            var transactionController = new TransactionController(context);
            var transaction = new Transaction
            {
                DescriptionOfTransaction = "Description",
                Budget = budgetName,
                ValueOfTransaction = 123
            };
            var view = transactionController.Create(transaction).Result as ViewResult;
        }

        CreateViewDataModel();
        _table.TransactionsPerCategories.Select(t => t.Transactions[0].Budget).Should().NotContain(budgetName);
    }

    [Fact]
    public void Create_ShouldIncludeAnyOtherCategory()
    {
        const string budgetName = "Flugzeugtraeger";

        using (var context = new BudgetDataContext(Options))
        {
            var transactionController = new TransactionController(context);
            var transaction = new Transaction
            {
                DescriptionOfTransaction = "Description",
                Budget = budgetName,
                ValueOfTransaction = 123
            };
            var view = transactionController.Create(transaction).Result as ViewResult;
        }

        CreateViewDataModel();
        _table.TransactionsPerCategories.Select(t => t.Transactions[0].Budget).Should().Contain(budgetName);
    }

    public void Dispose()
    {
        using (var context = new BudgetDataContext(Options))
        {
            var transaction = context.Transaction.ToList();
            if (transaction != null && transaction.Count > 0)
            {
                context.Transaction.RemoveRange(transaction);
                context.SaveChangesAsync();
            }
        }
    }
}