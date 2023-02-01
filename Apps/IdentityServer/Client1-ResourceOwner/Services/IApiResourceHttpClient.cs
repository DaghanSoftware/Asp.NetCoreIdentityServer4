using System.Net.Http;
using System.Threading.Tasks;
namespace IdentityServer.Client1ResourceOwner.Services
{
    public interface IApiResourceHttpClient
    {
       Task<HttpClient> GetHttpClient();
    }

}

