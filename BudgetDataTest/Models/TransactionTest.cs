using System;
using System.Numerics;
using Xunit;
using BudgetData.Models;
using FluentAssertions;

namespace BudgetDataTest;

public class TransactionTest
{
    [Fact]
    public void TransactionToString_shouldReturnFormattedString()
    {
        Transaction testTransaction = new()
        {
            Id = 1,
            DateOfTransaction = new DateTime(2022,12,12),
            DescriptionOfTransaction = "Ein Test",
            Budget = "Testbudget",
            ValueOfTransaction = 3
        };
    
        testTransaction.ToString().Should().Be("12.12.2022\t Ein Test\t 3,00 €");
    }
 }