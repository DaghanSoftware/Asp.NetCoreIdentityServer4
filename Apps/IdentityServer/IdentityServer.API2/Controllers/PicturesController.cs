using IdentityServer.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>()
            {
                new Picture{Id=1,Name="Url Resmi",Url="https://cdn.wmaraci.com/nedir/url.png"},
                new Picture{Id=2,Name="Nitro Method Resmi",Url="https://cdn.itemsatis.com/uploads/post_images/boost-nitro-sure-uzat-rozet-url-method-2916154.png"},
                new Picture{Id=3,Name="Aktif Developer Rozet Resmi",Url="https://cdn.itemsatis.com/uploads/post_images/kampanya-discord-active-developer-rozeti-49712958.png"}
            };
            return Ok(pictures);
        }
    }
}
