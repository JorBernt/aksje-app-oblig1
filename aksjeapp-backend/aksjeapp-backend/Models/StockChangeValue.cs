using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.Models
{
    public class StockChangeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }


        private double change;
        private double value;
        public double Change
        {
            get
            {
                return this.change;
            }
            set
            {
                this.change = Math.Round(value, 2);
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = Math.Round(value, 2);
            }
        }
    }
}
