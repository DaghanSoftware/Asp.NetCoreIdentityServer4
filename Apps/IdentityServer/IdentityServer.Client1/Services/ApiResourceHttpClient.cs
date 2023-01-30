using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Client1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _client;
        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
        }

        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            _client.SetBearerToken(accessToken);
            return _client;
        }
    }
}
