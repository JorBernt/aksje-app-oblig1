namespace aksjeapp_backend.DAL
{

    public interface IClientRepository
    {
        Task<bool> RegisterCustomer(Customer customer);
    }
}