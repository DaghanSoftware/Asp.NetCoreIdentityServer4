using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace IdentityServer.Client1ResourceOwner.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();
            var discoveryEndpoint = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7139");
            if (discoveryEndpoint.IsError)
            {
                //loglama yap
            }

            //Authentication property’ler içerisinden refresh token değeri elde edilmiştir.
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            //Refresh token ile request yapabilmemiz için ilgili client’ın bilgilerini tutan nesne oluşturulmuştur. Burada dikkat ederseniz kullanıcının username ve password bilgileri girilmemektedir.
            //Bunun nedeni, ilgili dataların cookie’de tutuluyor ve talep neticesinde IdentityServer4’e gönderiliyor olmasıdır.
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.ClientId = _configuration["Client1ResourceOwnerMvc:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["Client1ResourceOwnerMvc:ClientSecret"];
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = discoveryEndpoint.TokenEndpoint;

            //‘RefreshTokenRequest’ nesnesini kullanarak IdentityServer4 sunucusuna ‘grant_type’ değeri ‘refresh_token’ olan bir istek gönderir ve
            //neticede ‘TokenResponse’ olarak yeni access token, id token ve refresh token değerlerini elde eder.
            var token =await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                //hata sayfasına yönlendirme yap.
            }
            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();

            //Mevcudiyetteki tüm authentication property’ler elde edilir.
            var properties = authenticationResult.Properties;

            //Mevcudiyette elde edilen authentication property’ler ile talep neticesinde üretilen yeni değerleri ‘StoreTokens’ fonksiyonu ile değiştirmekteyiz.
            properties.StoreTokens(tokens);

            //İlgili kullanıcı yeni authentication property’ler ile güncellenmekte ve bunun için tekrar ilgili değerler eşliğinde SignIn yaptırılmaktadır.
            await HttpContext.SignInAsync("Cookies",authenticationResult.Principal,properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanelIndex()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Customer")]
        public IActionResult CustomerIndex()
        {
            return View();
        }
    }
}
