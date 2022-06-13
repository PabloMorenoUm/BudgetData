using System;
using System.Diagnostics;
using BudgetData.Data;
using BudgetData.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetDataTest.Controllers;

public class DatabaseFixture : IDisposable
{
    
    public DbContextOptions<BudgetDataContext> Options { get; }

    public DatabaseFixture()
    {
        Options = new DbContextOptionsBuilder<BudgetDataContext>()
            .UseInMemoryDatabase(databaseName: "BudgetDatabase")
            .Options;
        using (var context = new BudgetDataContext(Options))
        {
            context.Transaction!.Add(new Transaction()
                {DescriptionOfTransaction = "abc", Budget = "Budget1", ValueOfTransaction = (decimal) 0.01});
            context.Transaction.Add(new Transaction()
                {DescriptionOfTransaction = "bcd", Budget = "Budget1", ValueOfTransaction = (decimal) 0.10});
            context.Transaction.Add(new Transaction()
                {DescriptionOfTransaction = "cde", Budget = "Budget2", ValueOfTransaction = (decimal) 1.00});
            context.Transaction.Add(new Transaction()
                {DescriptionOfTransaction = "xyz", Budget = "Hifi", ValueOfTransaction = (decimal) 10.00});
            context.SaveChanges();
        }
    }

    public void Dispose()
    {
        
    }
}