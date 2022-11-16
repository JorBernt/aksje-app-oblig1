using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace aksjeapp_backend.Models
{
    public class User
    {

        [JsonProperty("username")]
        [RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ. \-]{5,20}$")]
        public string Username { get; set; }
        [JsonProperty("password")]
        //[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]){3,}$")]
        [RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ. \-]{3,}$")] // Not a secure regular expression
        public string Password { get; set; }
    }
}
