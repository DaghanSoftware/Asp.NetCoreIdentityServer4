using IdentityServer.IdentityApi_AuthServer.Models;
using IdentityServer.IdentityApiAuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.IdentityApiAuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(LocalApi.PolicyName)]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManger;

        public UserController(UserManager<ApplicationUser> userManger)
        {
            _userManger = userManger;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSaveViewModel userSaveViewModel)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = userSaveViewModel.UserName;
            applicationUser.Email = userSaveViewModel.Email;
            applicationUser.City= userSaveViewModel.City;
            var result = await _userManger.CreateAsync(applicationUser,userSaveViewModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }
            return Ok("Üye başarıyla kaydedildi.");
        }
    }
}
