using System.ComponentModel.DataAnnotations;

namespace BudgetData.Models;

public class IncomeTransactionsViewModel
{
    public List<Budget> BudgetList { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Gesamteinkommen")]
    public decimal TotalIncome { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Miete")]
    public decimal Miete { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Essen")]
    public decimal Essen { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Freizeit")]
    public decimal FreizeitIncome { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Sonstiges")]
    public decimal SonstigesIncome => TotalIncome - Miete - Essen - FreizeitIncome;

    public IEnumerable<Transaction> TransactionList
    {
        get
        {
            return new List<Transaction>()
            {
                createTransaction("Miete", Miete),
                createTransaction("Essen", Essen)
            };
        }
    }

    private Transaction createTransaction(string budget, decimal amount)
    {
        return new Transaction()
        {
            Budget = budget,
            DateOfTransaction = DateTime.Today,
            DescriptionOfTransaction = "Gehalt",
            ValueOfTransaction = amount
        };
    }
}