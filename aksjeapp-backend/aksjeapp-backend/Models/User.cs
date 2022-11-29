using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace aksjeapp_backend.Models
{
    public class User
    {

        [RegularExpression(@"^[0-9]{11}?$")] // Will accept either string of 11 numbers or empty string (which we need for update customer
        public string? Username { get; set; }
        
        [RegularExpression(@"^(?=.*[a-zA-ZæøåÆØÅ])(?=.*\d)[a-zA-ZæøåÆØÅ\d]{8,}")]
        public string Password { get; set; }
    }
}
