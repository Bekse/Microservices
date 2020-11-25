using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockTaxingControl.Models
{
    public class Trader
    {
        public int Id { get; set; }
        public decimal Wallet { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsTax { get; set; }
    }
}
