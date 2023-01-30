using System.Net.Http;
using System.Threading.Tasks;
namespace IdentityServer.Client1.Services
{
    public interface IApiResourceHttpClient
    {
       Task<HttpClient> GetHttpClient();
    }

}

