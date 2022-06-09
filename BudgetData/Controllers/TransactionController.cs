using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetData.Data;
using BudgetData.Models;

namespace BudgetData.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BudgetDataContext _context;
        private const string budgetCatAll = "Alle";

        public TransactionController(BudgetDataContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index(string budgetFilter, string searchString)
        {
            var transactions = from m in _context.Transaction
                select m;
            
            var allBudgets = transactions.Select(transaction => transaction.Budget).Distinct();

            var transactionTableViewModel = new TransactionsTableViewModel()
            {
                TotalSum = 0,
                TransactionsPerCategories = new List<TransactionsPerCategory>(),
                Budgets = new SelectList(allBudgets.ToList().Append(budgetCatAll).OrderBy(i => i))
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                transactions = transactions.Where(transaction =>
                    transaction.DescriptionOfTransaction.Contains(searchString));
            }
            
            var filteredBudgets = transactions.Select(transaction => transaction.Budget).Distinct();
            
            if (!String.IsNullOrEmpty(budgetFilter) && budgetFilter != budgetCatAll)
            {
                filteredBudgets = filteredBudgets.Where(budget => budget == budgetFilter);
            }
            
            foreach (var budget in filteredBudgets)
            {
                var _transactions = transactions.Where(transactions => transactions.Budget == budget);
                var _totalSum = _transactions.Select(t => t.ValueOfTransaction).Sum();
                var transactionsPerCategory = new TransactionsPerCategory()
                {
                    Transactions = _transactions.ToList(),
                    TotalSum = _totalSum
                };
                transactionTableViewModel.TransactionsPerCategories.Add(transactionsPerCategory);
                transactionTableViewModel.TotalSum += transactionsPerCategory.TotalSum;
            }
            
            return _context.Transaction != null ? 
                          View(transactionTableViewModel) :
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
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
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

            if (ModelState.IsValid)
            {
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
            return View(transaction);
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
    }
}
