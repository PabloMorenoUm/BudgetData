using System.ComponentModel.DataAnnotations;

namespace BudgetData.Models;

public class IncomeTransactions
{
    [DataType(DataType.Currency)]
    [Display(Name = "Gesamteinkommen")]
    public decimal TotalIncome { get; set; }
    
    [DataType(DataType.Currency)]
    [Display(Name = "Miete")]
    public decimal MieteIncome { get; set; }
    
    [DataType(DataType.Currency)]
    [Display(Name = "Essen")]
    public decimal EssenIncome { get; set; }
    
    [DataType(DataType.Currency)]
    [Display(Name = "Freizeit")]
    public decimal FreizeitIncome { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Sonstiges")]
    public decimal SonstigesIncome => TotalIncome - MieteIncome - EssenIncome - FreizeitIncome;

    public IEnumerable<Transaction> TransactionList
    {
        get
        {
            return new List<Transaction>()
            {
                createTransaction("Miete", MieteIncome),
                createTransaction("Essen", EssenIncome),
                createTransaction("Freizeit", FreizeitIncome),
                createTransaction("Sonstiges", SonstigesIncome)
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