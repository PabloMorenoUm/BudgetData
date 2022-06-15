using System.ComponentModel.DataAnnotations;

namespace BudgetData.Models;

public class Budget
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Kategorie")]
    public string Purpose { get; set; }
    
    [DataType(DataType.Currency)]
    [Display(Name = "Default Einkommen")]
    public decimal DefaultIncome { get; set; }
}