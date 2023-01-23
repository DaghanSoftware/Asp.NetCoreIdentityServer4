using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productsList = new List<Product>() { 
            new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 1242 },
            new Product { Id = 2, Name = "Kağıt", Price = 214, Stock = 125 },
            new Product { Id = 3, Name = "Defter", Price = 346, Stock = 1256 },
            new Product { Id = 4, Name = "Silgi", Price = 457, Stock = 1611 }
            };
            return Ok(productsList);
        }
    }
}
