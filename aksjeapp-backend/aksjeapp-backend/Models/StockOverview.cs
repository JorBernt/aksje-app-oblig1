namespace aksjeapp_backend.Models
{


    public class StockOverview
    {
        public string Symbol { get; set; }
        public string? Name { get; set; }

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