using System.ComponentModel.DataAnnotations;

namespace StockShareTraders.Models
{
    public class Trader
    {
        [Key]
        public int Id { get; set; }
        public decimal Wallet { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsTax { get; set; }
    }
}
