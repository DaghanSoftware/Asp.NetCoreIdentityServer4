using IdentityServer.AuthServer.Models;

namespace IdentityServer.AuthServer.Repository
{
    public interface ICustomUserRepository
    {
        Task<bool> Validate(string email, string password);
        Task<CustomUser> FindById(int id);
        Task<CustomUser> FindByEmail(string email);
    }
}
