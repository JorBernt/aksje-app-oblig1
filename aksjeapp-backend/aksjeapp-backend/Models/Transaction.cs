namespace aksjeapp_backend.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int Awaiting { get; set; } = 0; // Fjerne hvis det ikke går å kalle i fremtiden.
        public string Symbol { get; set; }
        public int Number {get; set; }
        public double PricePrStock { get; set; } // Mulig vi ikke trenger denne

        public double TotalPrice { get; set; }  

    }
}
