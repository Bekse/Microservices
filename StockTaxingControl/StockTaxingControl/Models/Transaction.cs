using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockTaxingControl.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }
        public int ShareId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
