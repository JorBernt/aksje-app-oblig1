using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace aksjeapp_backend.Models
{
    public class User
    {

        [JsonProperty("username")]
        [RegularExpression(@"^[0-9]{11}?$")] // Will accept either string of 11 numbers or empty string (which we need for update customer
        public string? Username { get; set; }
        [JsonProperty("password")]
        //[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]){3,}$")]
        //[RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ#?!@$%^&*-. \-]{3,}$")] // Not a secure regular expression
        [RegularExpression(@"^(?=.*[a-zA-ZæøåÆØÅ])(?=.*\d)[a-zA-ZæøåÆØÅ\d]{8,}")]
        public string Password { get; set; }
    }
}
