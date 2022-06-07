using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BudgetData.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BudgetData.Data
{
    public class BudgetDataContext : DbContext
    {
        public BudgetDataContext (DbContextOptions<BudgetDataContext> options)
            : base(options)
        {
        }

        public DbSet<BudgetData.Models.Transaction>? Transaction { get; set; }
    }
}
