using BudgetData.Data;
using BudgetData.Models;
using BudgetData.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetData.Controllers;

public class KontoServiceController : Controller
{
    private readonly BudgetDataContext _context;
    private KassensturzService _kassensturzService;
    private TransactionService _transactionService;

    public KontoServiceController(BudgetDataContext context)
    {
        _context = context;
        _kassensturzService = new KassensturzService(_context);
        _transactionService = new TransactionService(_context);
    }

    // GET
    public IActionResult Kassensturz()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Kassensturz([Bind("Konto, Bargeld")] Kassensturz kassensturz)
    {
        kassensturz.Difference = _kassensturzService.CalculateDifference(kassensturz);
        return View(kassensturz);
    }

    public IActionResult Gehaltseingang()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Gehaltseingang(
        [Bind("TotalIncome, MieteIncome, EssenIncome, FreizeitIncome")]
        IncomeTransactions incomeTransactions)
    {
        if (ModelState.IsValid)
        {
            _transactionService.BookIncomeTransactions(incomeTransactions);
            return Redirect(
                Url.Link("DefaultApi", new {controller = "Transaction", action = "Index"}) ?? "/"
            );
        }

        return View(incomeTransactions);
    }
}