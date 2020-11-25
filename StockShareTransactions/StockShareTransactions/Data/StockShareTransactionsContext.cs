using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockShareTransactions.Models;

namespace StockShareTransactions.Data
{
    public class StockShareTransactionsContext : DbContext
    {
        public StockShareTransactionsContext (DbContextOptions<StockShareTransactionsContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
