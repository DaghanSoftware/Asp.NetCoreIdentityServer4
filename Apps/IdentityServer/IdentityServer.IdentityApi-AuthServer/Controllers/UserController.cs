using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.IdentityApiAuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(LocalApi.PolicyName)]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignUp()
        {
            return Ok("Signup Çalıştı");
        }
    }
}
