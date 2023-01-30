using IdentityModel.Client;
using IdentityServer.Client1.Models;
using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Net.Http;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;
        public ProductsController(IConfiguration configuration,IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();

            //HttpClient httpCilent = new HttpClient();

            //var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //httpCilent.SetBearerToken(accessToken);
            //var response = await httpCilent.GetAsync("https://localhost:7014/api/Product/GetProducts");

            HttpClient client = await _apiResourceHttpClient.GetHttpClient();
            var response = await client.GetAsync("https://localhost:7014/api/Product/GetProducts");
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
