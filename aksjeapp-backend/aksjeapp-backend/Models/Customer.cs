using aksjeapp_backend.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace aksjeapp_backend.DAL
{
    public class Customer
    {
        [JsonProperty("SocialSecurityNumber")]
        [RegularExpression(@"^[0-9]{0,11}$")]
        public string? SocialSecurityNumber { get; set; }
        
        [JsonProperty("FirstName")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string? FirstName { get; set; }
        
        [JsonProperty("LastName")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string? LastName { get; set; }
        
        [JsonProperty("Address")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ0-9. \-]{2,50}$")]
        public string? Address { get; set; }
        
        private double _balance;
        public double Balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = Math.Round(value, 2);
            }
        }

        public List<Transaction>? Transactions { get; set; }
        
        [JsonProperty("PostalCode")]
        [RegularExpression(@"^[0-9]{4}$")]
        public string? PostalCode { get; set; }
        
        [JsonProperty("PostCity")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,50}$")]
        public string? PostCity { get; set; }

        public Portfolio? Portfolio { get; set; }
        
        public User? User { get; set; }
 
    }
}
