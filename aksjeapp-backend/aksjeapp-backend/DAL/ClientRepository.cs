namespace aksjeapp_backend.DAL;

public class ClientRepository : IClientRepository
{
    private readonly StockContext _db;

    public ClientRepository(StockContext db)
    {
        _db = db;
    }
}