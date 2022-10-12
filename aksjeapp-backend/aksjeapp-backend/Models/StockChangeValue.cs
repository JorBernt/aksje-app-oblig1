using System.ComponentModel.DataAnnotations;

namespace aksjeapp_backend.Models
{
    public class StockChangeValue
    {
        [Key]
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public double Change { get; set; }
        public double Value { get; set; }
    }
}
