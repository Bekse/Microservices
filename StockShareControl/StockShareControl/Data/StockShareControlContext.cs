using Microsoft.EntityFrameworkCore;
using StockShareControl.Models;

namespace StockShareControl.Data
{
    public class StockShareControlContext : DbContext
    {
        public StockShareControlContext (DbContextOptions<StockShareControlContext> options)
            : base(options)
        {
        }

        public DbSet<Share> Shares { get; set; }
    }
}
