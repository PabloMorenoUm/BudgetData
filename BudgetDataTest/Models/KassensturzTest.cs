using BudgetData.Models;
using FluentAssertions;
using Xunit;

namespace BudgetDataTest;

public class KassensturzTest
{
    private Kassensturz _kassensturz;

    public KassensturzTest()
    {
        _kassensturz = new()
        {
            Konto = 1,
            Bargeld = 2
        };
    }

    [Fact]
    public void Sum_ShouldAddKontoAndBargeld()
    {
        _kassensturz.Sum.Should().Be(3);
    }
}