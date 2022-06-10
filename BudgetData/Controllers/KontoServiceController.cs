using Microsoft.AspNetCore.Mvc;

namespace BudgetData.Controllers;

public class KontoServiceController : Controller
{
    // GET
    public IActionResult Kassensturz()
    {
        return View();
    }
    
    public IActionResult Gehaltseingang()
    {
        return View();
    }
}