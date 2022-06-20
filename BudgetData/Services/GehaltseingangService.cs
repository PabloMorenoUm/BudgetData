namespace BudgetData.Services;

public class GehaltseingangService
{
    public Dictionary<string, decimal> PrepareDict(Dictionary<string, string> incomeTransactions)
    {
        if (incomeTransactions["__RequestVerificationToken"] != null)
        {
            incomeTransactions.Remove("__RequestVerificationToken");
        }

        Dictionary<string, decimal> resultingTransactions = new Dictionary<string, decimal>();

        foreach (var key in incomeTransactions.Keys)
        {
            decimal amount = decimal.Parse(incomeTransactions[key].Split(' ')[0]);
            resultingTransactions.Add(key, amount);
        }
        resultingTransactions.Add("Freizeit", calculateFreizeitBudget(resultingTransactions));
        resultingTransactions.Remove("Gesamteinkommen");
        return resultingTransactions;
    }

    private decimal calculateFreizeitBudget(Dictionary<string, decimal> resultingTransactions)
    {
        decimal totalSum = resultingTransactions.Values.Sum();
        return totalSum - 2 * resultingTransactions["Gesamteinkommen"];
    }
}