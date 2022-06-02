using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace BudgetData.Models;

public class Transaction
{
    public int Id { get; set; }
    
    [Display(Name = "Datum")]
    [DataType(DataType.Date)]
    public DateTime DateOfTransaction { get; set; }
    
    [StringLength(60, MinimumLength = 3)]
    [Display(Name = "Beschreibung")]
    public string DescriptionOfTransaction { get; set; }
    public string Budget { get; set; }
    
    [DataType(DataType.Currency)]
    [Display(Name = "Geldbetrag")]
    public decimal ValueOfTransaction { get; set; }

    public override string ToString()
    {
        return $"{DateOfTransaction:dd.MM.yyyy}\t {DescriptionOfTransaction}\t {ValueOfTransaction:C}";
    }
}