using aksjeapp_backend.Models;

namespace aksjeapp_backend.DAL
{
    public class Customer
    {
        public string SocialSecurityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public double Balance { get; set; } = 0;
        public List<Transaction> Transactions { get; set; }
        public string PostalCode { get; set; }
        public string PostCity { get; set; }
    }
}
