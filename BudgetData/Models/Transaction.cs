using System.Numerics;

namespace BudgetData.Models;

public class Transaction
{
    public int Id { get; set; }
    public DateTime DateOfTransaction { get; set; }
    public string DescriptionOfTransaction { get; set; }
    public string Budget { get; set; }
    public decimal ValueOfTransaction { get; set; }

    public override string ToString()
    {
        return $"{DateOfTransaction:dd.MM.yyyy}\t {DescriptionOfTransaction}\t {ValueOfTransaction:C}";
    }
}