using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockShareTransactions.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int ShareId { get; set; }
        public int SellerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
    }
}
