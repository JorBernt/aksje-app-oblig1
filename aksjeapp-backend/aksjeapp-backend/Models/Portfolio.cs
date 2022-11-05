using aksjeapp_backend.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.Models
{
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PortfolioId { get; set; }


        public string SocialSecurityNumber { get; set; }

        public virtual List<PortfolioList> StockPortfolio { get; set; }

        private double value;
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
