namespace aksjeapp-backend.DAL
{
    public class Customer
{
    public int SocialSecurityNumber { get; set; }   
    string FirstName { get; set; }
    string LastName { get; set; }
    string Address { get; set; }
    List<Transaction> Transactions { get; set; }
    string PostalCode { get; set; }
    string PostCity { get; set; }
}
}
