using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Discounts.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("DShop Discounts Service");

        [HttpGet("ping")]
        public IActionResult Ping() => Ok();
    }
}