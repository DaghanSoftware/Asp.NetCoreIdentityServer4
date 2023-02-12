using Client1_ResourceOwner.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace Client1_ResourceOwner.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            //Klasik discovery endpoint‘e istek gönderilmekte ve token talebi için gerekli request nesnesi(PasswordTokenRequest) üretilmektedir.
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);
            if (disco.IsError)
            {
                //hata yakalama ve loglama
            }
            var password = new PasswordTokenRequest();
            password.Address = disco.TokenEndpoint;
            password.UserName = model.Email;
            password.Password = model.Password;
            password.ClientId = _configuration["Client1ResourceOwnerMvc:ClientId"];
            password.ClientSecret = _configuration["Client1ResourceOwnerMvc:ClientSecret"];


            //Auth Server’a ‘RequestPasswordTokenAsync’ fonksiyonu ile resource owner credential türünde istek yapılmakta ve token talep edilmektedir.
            var token = await client.RequestPasswordTokenAsync(password);
            if (token.IsError)
            {
                ModelState.AddModelError("", "Email veya şifreniz yanlış");
                return View();
            }

            //Elde edilen token’da ki access token değeri ile kullanıcı bilgileri talep edilmektedir.
            var userinfoRequest = new UserInfoRequest();
            userinfoRequest.Token = token.AccessToken;
            userinfoRequest.Address = disco.UserInfoEndpoint;
            var userinfo = await client.GetUserInfoAsync(userinfoRequest);
            if (userinfo.IsError)
            {
                //hata fırlat ve logla
            }

            //Kullanıcı bilgileri tutacak olan ‘ClaimsPrincipal’ nesnesi oluşturulmaktadır.
            //Burada dikkat edilmesi gereken husus 67. satırda üretilen new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            //nesnesinin ilk parametresi dışında 2. , 3. ve 4. parametreleridir. 2. parametre, ilgili uygulamanın kullandığı cookie adını gerektirirken(aşağıdaki satırlarda göreceğiz),
            //3. parametre User.Identity.Name koduna karşılık kullanıcıdan gelen hangi claim ile eşleştirilmesi gerektiğini bildirmekte ve
            //4. parametre ise benzer şekilde kullanıcının authorize/rol yetkisinin yine kullanıcıdan gelen claim’lerden hangisiyle eşleşmesi gerektiğini belirlemektedir.
            //Burada ilgili claim değerlerinin elde edilebilmesi için Auth Server’da aşağıdaki Identity Resource’lerin eklenmesi gerektiğini unutmayınız…
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userinfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //Burada giriş yapan kullanıcının elde ettiği authentication properties bilgilerini client uygulaması üzerinde güncellemekteyiz.
            //Hatta hatırlarsanız IdentityServer4 mimarisi için refresh token konusunu ele alırken de birebir aynı işlemi gerçekleştirmiştik.
            //Lakin ilgili makalede authentication property’leri
            //(await HttpContext.AuthenticateAsync()).Properties; kodu ile elde ediyor, üzerine değişikliklerimizi gerçekleştiriyorduk.
            //Halbuki burada direkt olarak ‘AuthenticationProperties’ türünden nesneyi elimizle manuel oluşturuyoruz.
            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            //Giriş işlemini yukarıda üretilen değerler eşliğinde gerçekleştiriyoruz.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            return RedirectToAction("Index", "User");
        }

        public IActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSaveViewModel userSaveViewModel)
        {
            return RedirectToAction("Index");
        }
    }
}
