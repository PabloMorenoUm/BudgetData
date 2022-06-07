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

public class TransactionControllerTest
{
    [Fact]
    public void Index_ShouldCalculateTotalSumOfAllTransactions()
    {
        //arrange
        var options = new DbContextOptionsBuilder<BudgetDataContext>()
            .UseInMemoryDatabase(databaseName: "BudgetDatabase")
            .Options;

        using (var context = new BudgetDataContext(options))
        {
            context.Transaction.Add(new Transaction()
                {Id = 1, DescriptionOfTransaction = ".", Budget = "Kategorie1", ValueOfTransaction = (decimal) 20.20});
            context.Transaction.Add(new Transaction()
                {Id = 2, DescriptionOfTransaction = ".", Budget = "Kategorie1", ValueOfTransaction = (decimal) 200});
            context.Transaction.Add(new Transaction()
                {Id = 3, DescriptionOfTransaction = ".", Budget = "Kategorie2", ValueOfTransaction = (decimal) 2.02});
            context.SaveChanges();
        }


        //act
        TransactionsTableViewModel table;
        using (var context = new BudgetDataContext(options))
        {
            TransactionController transactionController = new TransactionController(context);
            var view = transactionController.Index().Result as ViewResult;
            table = (TransactionsTableViewModel) view.ViewData.Model;
        }
        

        //assert
        table.TotalSum.Should().Be((decimal) 222.22);
    }
}