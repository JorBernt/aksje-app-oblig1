using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.Models
{
    public class StockChangeValue
    {

        [Key, Column(Order = 0)]
        public string Date { get; set; }
        [Key, Column(Order = 1)]
        public string Symbol { get; set; }
        public double Change { get; set; }
        public double Value { get; set; }
    }
}
