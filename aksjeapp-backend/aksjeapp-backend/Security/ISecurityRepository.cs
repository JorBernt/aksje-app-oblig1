using aksjeapp_backend.DAL;

namespace aksjeapp_backend.Security
{
    public interface ISecurityRepository
{
        Task<string> LoggedIn(ISession session);
        Task<bool> LogIn(Customer user, ISession session);
        Task<bool> LogOut(Customer user, ISession session);
    }
}
