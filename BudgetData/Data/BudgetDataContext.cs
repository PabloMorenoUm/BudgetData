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

        public DbSet<Transaction>? Transaction { get; set; }

        public DbSet<Budget>? Budget { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var decimalProps = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
