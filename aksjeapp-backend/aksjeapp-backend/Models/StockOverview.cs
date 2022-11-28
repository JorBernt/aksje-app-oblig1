namespace aksjeapp_backend.Models
{


    public class StockOverview
    {
        public string Symbol { get; set; }
        public string? Name { get; set; }
        public double Value { get; set; }
        public double Change { get; set; }
    }

}