using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.Models
{
    public class StockChangeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string StockId { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }
        public double Change { get; set; }
        public double Value { get; set; }
    }
}
