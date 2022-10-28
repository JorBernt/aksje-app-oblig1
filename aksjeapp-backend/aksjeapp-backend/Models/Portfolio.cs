namespace aksjeapp_backend.Models
{
    public class Portfolio
    {
        public List<StockOverview> StockPortfolio { get; set; }

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
    }
}
