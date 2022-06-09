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

namespace BudgetDataTest.Controllers;

public class TransactionControllerTest : IClassFixture<DatabaseFixture>
{
    DatabaseFixture _fixture;
    TransactionsTableViewModel table;

    public TransactionControllerTest(DatabaseFixture fixture)
    {
        this._fixture = fixture;
    }

    private void createViewDataModel(string? budgetFilter = null, string? searchString = null)
    {
        using (var context = new BudgetDataContext(_fixture.Options))
        {
            TransactionController transactionController = new TransactionController(context);
            var view = transactionController.Index(budgetFilter, searchString).Result as ViewResult;
            table = (TransactionsTableViewModel)view.ViewData.Model;
        }
    }

    [Fact]
    public void Index_ShouldCalculateTotalSumOfAllTransactions()
    {
        createViewDataModel();
        table.TotalSum.Should().Be((decimal) 222.22);
    }

    [Fact]
    public void Index_ShouldContainTwoDistinctBudgets()
    {
        createViewDataModel();
        table.TransactionsPerCategories.Count.Should().Be(2);
    }

    [Fact]
    public void Index_ShouldCalculateSubtotalSumOfAnyBudget()
    {
        createViewDataModel();
        table.TransactionsPerCategories[0].TotalSum.Should().Be((decimal) 220.20);
        table.TransactionsPerCategories[0].Transactions.Count.Should().Be(2);
        table.TransactionsPerCategories[1].TotalSum.Should().Be((decimal) 2.02);
        table.TransactionsPerCategories[1].Transactions.Count.Should().Be(1);
    }

    [Fact]
    public void IndexWithFilter_ShouldContainOneBudget()
    {
        createViewDataModel("Budget1");
        table.TransactionsPerCategories.Count.Should().Be(1);
        table.TotalSum.Should().Be((decimal) 220.20);
    }

    [Fact]
    public void IndexWithFilter_ShouldContainAllBudgets()
    {
        createViewDataModel("Alle");
        table.TransactionsPerCategories.Count.Should().Be(2);
        table.TotalSum.Should().Be((decimal) 222.22);
    }

    [Fact]
    public void IndexWithSearch_ShouldContainMatchingTransactions()
    {
        createViewDataModel(searchString: "cd");
        table.TransactionsPerCategories.Count.Should().Be(2);
        table.TransactionsPerCategories[0].Transactions.Count.Should().Be(1);
    }

    [Fact]
    public void IndexWithFilterAndSearch_ShouldContainMatchingTransactions()
    {
        createViewDataModel("Budget1", "cd");
        table.TransactionsPerCategories.Count.Should().Be(1);
        table.TransactionsPerCategories[0].Transactions.Count.Should().Be(1);
    }
}