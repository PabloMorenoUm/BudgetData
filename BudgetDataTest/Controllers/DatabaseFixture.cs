using System;
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
                {Id = 1, DescriptionOfTransaction = "abc", Budget = "Budget1", ValueOfTransaction = (decimal) 20.20});
            context.Transaction.Add(new Transaction()
                {Id = 2, DescriptionOfTransaction = "bcd", Budget = "Budget1", ValueOfTransaction = (decimal) 200});
            context.Transaction.Add(new Transaction()
                {Id = 3, DescriptionOfTransaction = "cde", Budget = "Budget2", ValueOfTransaction = (decimal) 2.02});
            context.SaveChanges();
        }
    }

    public void Dispose()
    {
        // using (var context = new BudgetDataContext(Options))
        // {
        //     IEnumerable<Transaction> DbSet = context.Transaction.ToList();
        //     context.Transaction.RemoveRange(DbSet);
        //     context.SaveChanges();
        // }
    }
}