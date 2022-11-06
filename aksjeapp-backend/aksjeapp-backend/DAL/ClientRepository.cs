namespace aksjeapp_backend.DAL;

public class ClientRepository : IClientRepository
{
    private readonly StockContext _db;

    public ClientRepository(StockContext db)
    {
        _db = db;
    }

    public async Task<bool> RegisterCustomer(Customer customer)
    {
        try
        {
            var c = new Customers
            {
                SocialSecurityNumber = customer.SocialSecurityNumber,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Balance = 0,
                TransactionsBought = null,
                TransactionsSold = null,
                Portfolio = customer.Portfolio,
                PostalArea = new PostalAreas
                {
                    PostalCode = customer.PostalCode,
                    PostCity = customer.PostCity
                }
            };
            await _db.Customers.AddAsync(c);
        }
        catch (Exception exception)
        {
            return false;
        }
        return true;
    }
}