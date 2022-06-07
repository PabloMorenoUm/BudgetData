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

        using (var context = new BudgetDataContext(_fixture.Options))
        {
            TransactionController transactionController = new TransactionController(context);
            var view = transactionController.Index().Result as ViewResult;
            table = (TransactionsTableViewModel) view.ViewData.Model;
        }
    }

    [Fact]
    public void Index_ShouldCalculateTotalSumOfAllTransactions()
    {
        table.TotalSum.Should().Be((decimal) 222.22);
    }

    [Fact]
    public void Index_ShouldContainTwoDistinctBudgets()
    {
        table.TransactionsPerCategories.Count.Should().Be(2);
    }

    [Fact]
    public void Index_ShouldCalculateSubtotalSumOfAnyBudget()
    {
        table.TransactionsPerCategories[0].TotalSum.Should().Be((decimal) 220.20);
        table.TransactionsPerCategories[0].Transactions.Count.Should().Be(2);
        table.TransactionsPerCategories[1].TotalSum.Should().Be((decimal) 2.02);
        table.TransactionsPerCategories[1].Transactions.Count.Should().Be(1);
    }
}