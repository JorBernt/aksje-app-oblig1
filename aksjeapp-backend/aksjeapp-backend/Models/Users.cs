using System.ComponentModel.DataAnnotations;

namespace aksjeapp_backend.Models
{
    public class Users
{
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
