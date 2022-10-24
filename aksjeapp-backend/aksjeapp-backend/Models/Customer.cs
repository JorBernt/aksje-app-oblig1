using aksjeapp_backend.Models;

namespace aksjeapp_backend.DAL
{
    public class Customer
    {   
        [RegularExpression(@"^[0-9]{11}$")] 
        public string SocialSecurityNumber { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string LastName { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ0-9. \-]{2,50$")]
        public string Address { get; set; }
        public double Balance { get; set; } = 0;
        public List<Transaction> Transactions { get; set; }
        [RegularExpression(@"^[0-9]{4}$")]
        public string PostalCode { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,50$")]
        public string PostCity { get; set; }

        public Portofolio Portofolio { get; set; }
    }
}
