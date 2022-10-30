using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.Models
{
    public class Transaction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Lage et tilfeldig nummer

        [RegularExpression(@"^[0-9]{11}$")]
        public string SocialSecurityNumber { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }
        [RegularExpression(@"^^(?!0*[.]0*$|[.]0*$|0*$)\d+[.]?\d{0,2}$")]
        public int Amount { get; set; }
        public double TotalPrice { get; set; }
        public bool Awaiting { get; set; }
    }
}
