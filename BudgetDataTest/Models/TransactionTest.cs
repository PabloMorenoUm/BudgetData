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
        Transaction testTransaction = new(new DateOnly(2022,12,12), "Ein Test", new BigInteger(3));

        testTransaction.ToString().Should().Be("12.12.2022\t Ein Test\t 3,00 €");
    }
 }