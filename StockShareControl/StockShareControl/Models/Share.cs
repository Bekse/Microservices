using System.ComponentModel.DataAnnotations;

namespace StockShareControl.Models
{
    public class Share
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public decimal Price { get; set; }
        public bool ForSale { get; set; }
    }
}
