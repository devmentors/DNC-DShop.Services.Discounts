using System.Threading.Tasks;
using DShop.Common.Dispatchers;
using DShop.Common.Mvc;
using DShop.Services.Discounts.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Discounts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public DiscountsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateDiscount command)
        {
            await _dispatcher.SendAsync(command.BindId(c => c.Id));

            return Accepted();
        }
    }
}