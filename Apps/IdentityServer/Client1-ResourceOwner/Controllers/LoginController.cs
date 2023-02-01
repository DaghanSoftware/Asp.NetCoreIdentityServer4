using Microsoft.AspNetCore.Mvc;

namespace Client1_ResourceOwner.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
