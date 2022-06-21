using System;
using System.Linq;
using BudgetData.Controllers;
using BudgetData.Data;
using BudgetData.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BudgetDataTest.Controllers;

public class KontoServiceControllerTest: IDisposable
{
    private DbContextOptions<BudgetDataContext> Options { get; }
    private Kassensturz _kassensturz;


    public KontoServiceControllerTest()
    {
        Options = new DbContextOptionsBuilder<BudgetDataContext>()
            .UseInMemoryDatabase(databaseName: "KontoServiceTestDatabase")
            .Options;

        using (var context = new BudgetDataContext(Options))
        {
            context.Transaction!.Add(new Transaction
                { DescriptionOfTransaction = "abc", Budget = "Budget1", ValueOfTransaction = (decimal)0.01 });
            context.Transaction.Add(new Transaction
                { DescriptionOfTransaction = "bcd", Budget = "Budget1", ValueOfTransaction = (decimal)0.10 });
            context.Transaction.Add(new Transaction
                { DescriptionOfTransaction = "cde", Budget = "Budget2", ValueOfTransaction = (decimal)1.00 });
            context.Transaction.Add(new Transaction
                { DescriptionOfTransaction = "xyz", Budget = "Hifi", ValueOfTransaction = (decimal)10.00 });
            context.SaveChanges();
        }
    }

    private void PrepareTests()
    {
        using (var context = new BudgetDataContext(Options))
        {
            var kassensturz = new Kassensturz
            {
                Konto = 1,
                Bargeld = 10
            };
            var kontoServiceController = new KontoServiceController(context);
            var view = kontoServiceController.Kassensturz(kassensturz) as ViewResult;
            _kassensturz = (Kassensturz)view.ViewData.Model;
        }
    }

    [Fact]
    public void CalculateDifference_ShouldSubtractKassensturzValuesFromDatabaseValues()
    {
        PrepareTests();
        _kassensturz.Difference.Should().Be((decimal) .11);
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