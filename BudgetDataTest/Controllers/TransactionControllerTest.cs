using System;
using System.Collections.Generic;
using System.Linq;
using BudgetData.Controllers;
using BudgetData.Data;
using BudgetData.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace BudgetDataTest.Controllers;

public class TransactionControllerTest : IClassFixture<DatabaseFixture>
{
    DatabaseFixture _fixture;
    TransactionsTableViewModel _table;

    public TransactionControllerTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private void CreateViewDataModel(string? budgetFilter = null, string? searchString = null)
    {
        using (var context = new BudgetDataContext(_fixture.Options))
        {
            var transactionController = new TransactionController(context);
            var view = transactionController.Index(budgetFilter, searchString).Result as ViewResult;
            _table = (TransactionsTableViewModel)view.ViewData.Model;
        }
    }

    [Fact]
    public void Index_ShouldCalculateTotalSumOfAllTransactions()
    {
        CreateViewDataModel();
        _table.TotalSum.Should().Be((decimal)222.22);
    }

    [Fact]
    public void Index_ShouldContainTwoDistinctBudgets()
    {
        CreateViewDataModel();
        _table.TransactionsPerCategories.Count.Should().Be(2);
    }

    [Fact]
    public void Index_ShouldCalculateSubtotalSumOfAnyBudget()
    {
        CreateViewDataModel();
        _table.TransactionsPerCategories[0].TotalSum.Should().Be((decimal)220.20);
        _table.TransactionsPerCategories[0].Transactions.Count.Should().Be(2);
        _table.TransactionsPerCategories[1].TotalSum.Should().Be((decimal)2.02);
        _table.TransactionsPerCategories[1].Transactions.Count.Should().Be(1);
    }

    [Fact]
    public void IndexWithFilter_ShouldContainOneBudget()
    {
        CreateViewDataModel("Budget1");
        _table.TransactionsPerCategories.Count.Should().Be(1);
        _table.TotalSum.Should().Be((decimal)220.20);
    }

    [Fact]
    public void IndexWithFilter_ShouldContainAllBudgets()
    {
        CreateViewDataModel("Alle");
        _table.TransactionsPerCategories.Count.Should().Be(2);
        _table.TotalSum.Should().Be((decimal)222.22);
    }

    [Fact]
    public void AIndexWithSearch_ShouldContainMatchingTransactions()
    {
        CreateViewDataModel(searchString: "cd");
        _table.TransactionsPerCategories.Count.Should().Be(2);
        _table.TransactionsPerCategories[0].Transactions.Count.Should().Be(1);
    }

    [Fact]
    public void IndexWithFilterAndSearch_ShouldContainMatchingTransactions()
    {
        CreateViewDataModel("Budget1", "cd");
        _table.TransactionsPerCategories.Count.Should().Be(1);
        _table.TransactionsPerCategories[0].Transactions.Count.Should().Be(1);
    }

    [Fact]
    public void Create_ShouldExcludeCategoryAll()
    {
        const string budgetName = TransactionService.BudgetCategoryAll;

        using (var context = new BudgetDataContext(_fixture.Options))
        {
            var transactionController = new TransactionController(context);
            var transaction = new Transaction
            {
                Id = 42,
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
        
        using (var context = new BudgetDataContext(_fixture.Options))
        {
            var transactionController = new TransactionController(context);
            var transaction = new Transaction
            {
                Id = 4,
                DescriptionOfTransaction = "Description",
                Budget = budgetName,
                ValueOfTransaction = 123
            };
            var view = transactionController.Create(transaction).Result as ViewResult;
        }

        CreateViewDataModel();
        _table.TransactionsPerCategories.Select(t => t.Transactions[0].Budget).Should().Contain(budgetName);
    }
}