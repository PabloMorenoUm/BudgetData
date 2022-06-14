using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetData.Data;
using BudgetData.Models;
using BudgetData.Services;

namespace BudgetData.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BudgetDataContext _context;
        private readonly TransactionService _transactionService;

        public TransactionController(BudgetDataContext context)
        {
            _context = context;
            _transactionService = new TransactionService(_context);
        }

        // GET: Transaction
        public async Task<IActionResult> Index(string budgetFilter, string searchString)
        {
            _transactionService.SearchTransactionsByDescription(searchString);
            _transactionService.FilterBudgets(budgetFilter);
            
            return _context.Transaction != null ? 
                          View(_transactionService.GenerateTablesPerBudget()) :
                          Problem("Entity set 'BudgetDataContext.Transaction'  is null.");
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOfTransaction,DescriptionOfTransaction,ValueOfTransaction,Budget")] Transaction transaction)
        {
            if (!ModelState.IsValid || transaction.Budget == TransactionService.BudgetCategoryAll)
                return View(transaction);
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfTransaction,DescriptionOfTransaction,ValueOfTransaction,Budget")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(transaction);
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transaction == null)
            {
                return Problem("Entity set 'BudgetDataContext.Transaction'  is null.");
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transaction?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async void DeleteAll()
        {
            _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Transaction");;
            
        }
    }
}
