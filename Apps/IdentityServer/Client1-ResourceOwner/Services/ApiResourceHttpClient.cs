using Client1_ResourceOwner.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text;

namespace IdentityServer.Client1ResourceOwner.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _client;
        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
            _configuration = configuration;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            _client.SetBearerToken(accessToken);
            return _client;
        }

        public async Task<List<string>> SaveUserViewModel(UserSaveViewModel userSaveViewModel)
        {
            var disco = await _client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

            if (disco.IsError)
            {
                //loglama yap
            }
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();
            clientCredentialsTokenRequest.ClientId = _configuration["Client1ResourceOwnerMvc:ClientId"];
            clientCredentialsTokenRequest.ClientSecret = _configuration["Client1ResourceOwnerMvc:ClientSecret"];
            clientCredentialsTokenRequest.Address = disco.TokenEndpoint;
            var token = await _client.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (token.IsError)
            {
                //loglama yap
            }
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userSaveViewModel), Encoding.UTF8, "application/json");
            _client.SetBearerToken(token.AccessToken);
            var response = await _client.PostAsync("https://localhost:7134/api/user/signup", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                var errorList = JsonConvert.DeserializeObject<List<string>> (await response.Content.ReadAsStringAsync());
                return errorList;
            }
            return null;
        }
    }
}
