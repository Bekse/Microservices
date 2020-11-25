using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTraderBroker.Models
{
    public class Share
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public decimal Price { get; set; }
        public bool ForSale { get; set; }
    }
}
