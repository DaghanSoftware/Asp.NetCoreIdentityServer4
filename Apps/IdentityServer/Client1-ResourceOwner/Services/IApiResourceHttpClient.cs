using Client1_ResourceOwner.Models;
using System.Net.Http;
using System.Threading.Tasks;
namespace IdentityServer.Client1ResourceOwner.Services
{
    public interface IApiResourceHttpClient
    {
       Task<HttpClient> GetHttpClient();
        Task<List<string>> SaveUserViewModel(UserSaveViewModel userSaveViewModel);
    }

}

