using System.Numerics;

namespace BudgetData.Models;

public record Transaction(DateOnly DateOfTransaction, string DescriptionOfTransaction, BigInteger ValueOfTransaction)
{
    public override string ToString()
    {
        return $"{DateOfTransaction:dd.MM.yyyy}\t {DescriptionOfTransaction}\t {ValueOfTransaction:C}";
    }
}