using BudgetData.Data;
using BudgetData.Models;
using BudgetData.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace BudgetData.Controllers;

public class KontoServiceController : Controller
{
    private readonly BudgetDataContext _context;
    private KassensturzService _kassensturzService;
    private TransactionService _transactionService;
    private GehaltseingangService _gehaltseingangService;

    public KontoServiceController(BudgetDataContext context)
    {
        _context = context;
        _gehaltseingangService = new GehaltseingangService();
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
        var budgets = from b in _context.Budget select b;
        BudgetListModel budgetList = new()
        {
            BudgetList = budgets.ToList()
        };

        return View(budgetList);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Gehaltseingang(Dictionary<string, string> incomeTransactions)
    {
        if (ModelState.IsValid)
        {
            var preparedIncomeTransactions = _gehaltseingangService.PrepareDict(incomeTransactions);
            _transactionService.BookIncomeTransactionsFromDict(preparedIncomeTransactions);
            return Redirect(
                Url.Link("DefaultApi", new {controller = "Transaction", action = "Index"}) ?? "/"
            );
        }

        return RedirectToAction("Gehaltseingang");
    }
}