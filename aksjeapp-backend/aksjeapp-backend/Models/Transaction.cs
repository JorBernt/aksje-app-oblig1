namespace aksjeapp_backend.Models
{
    public class Transaction
    {
        public string Id { get; set; } // Lage et tilfeldig nummer
        public string Date { get; set; }
        public string Symbol { get; set; }
        public int Number {get; set; }
        public double PricePrStock { get; set; } // Mulig vi ikke trenger denne

        public double TotalPrice { get; set; }

        public Transaction(string id, string date, string symbol, int number, double pricePrStock, double totalPrice)
        {
            Id = id;
            Date = date;
            Symbol = symbol;
            Number = number;
            PricePrStock = pricePrStock;
            TotalPrice = totalPrice;
        }
    }
}
