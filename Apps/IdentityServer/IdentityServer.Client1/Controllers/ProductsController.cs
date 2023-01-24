using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products= new List<Product>();

            HttpClient httpCilent = new HttpClient();
            var discoveryEndpoint = await httpCilent.GetDiscoveryDocumentAsync("https://localhost:7139");
            if (discoveryEndpoint.IsError)
            {
                //loglama yap
            }
            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();
            clientCredentialsTokenRequest.ClientId = _configuration["Client:ClientId"];
            clientCredentialsTokenRequest.ClientSecret = _configuration["Client:ClientSecret"];
            clientCredentialsTokenRequest.Address = discoveryEndpoint.TokenEndpoint;
            var token = await httpCilent.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);
            if (token.IsError)
            {
                //loglama yap
            }
            //https://localhost:7218

            httpCilent.SetBearerToken(token.AccessToken);
            var response = await httpCilent.GetAsync("https://localhost:7014/api/Product/GetProducts");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<Product>>(content);
            }
            else
            {

            }
            return View(products);
        }
    }
}
