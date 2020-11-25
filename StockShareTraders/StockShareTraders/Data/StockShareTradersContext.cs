using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockShareTraders.Models;

namespace StockShareTraders.Data
{
    public class StockShareTradersContext : DbContext
    {
        public StockShareTradersContext (DbContextOptions<StockShareTradersContext> options)
            : base(options)
        {
        }

        public DbSet<Trader> Traders { get; set; }
    }
}
