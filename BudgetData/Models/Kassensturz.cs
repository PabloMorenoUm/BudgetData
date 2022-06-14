using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BudgetData.Models;

public class Kassensturz
{
    [DataType(DataType.Currency)]
    public decimal Konto { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Bargeld { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Difference { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Sum => Konto + Bargeld;
}