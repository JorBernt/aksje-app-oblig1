namespace aksjeapp_backend.Models
{
    public class Portofolio
    {
        public string socialSecurityNumber { get; set; }
        public List<PortofoilioList> StockPortofolio { get; set; }
        public double Value { get; set; }
        public string LastUpdated { get; set; }
    }
}
