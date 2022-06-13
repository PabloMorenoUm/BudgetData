namespace BudgetData.Models;

public class Kassensturz
{
    public decimal Konto { get; set; }
    
    public decimal Bargeld { get; set; }
    
    public decimal Difference { get; set; }

    public decimal Sum => Konto + Bargeld;
}