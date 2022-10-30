using aksjeapp_backend.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace aksjeapp_backend.Models
{
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PortfolioId { get; set; }

        [NotNull]
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
        public string LastUpdated { get; set; }

        [ForeignKey("PortfolioList")]
        public int PortfolioListId { get; set; }
    }
}
