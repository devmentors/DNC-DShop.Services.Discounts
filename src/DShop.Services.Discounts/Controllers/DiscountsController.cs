using System.Collections.Generic;
using System.Threading.Tasks;
using DShop.Common.Dispatchers;
using DShop.Common.Mvc;
using DShop.Services.Discounts.Dto;
using DShop.Services.Discounts.Messages.Commands;
using DShop.Services.Discounts.Queries;
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

        // Idempotent
        // No side effects
        // Doesn't mutate a state
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountDto>>> Get([FromQuery] FindDiscounts query)
            => Ok(await _dispatcher.QueryAsync(query));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DiscountDetailsDto>> Get([FromRoute] GetDiscount query)
        {
            var discount = await _dispatcher.QueryAsync(query);
            if (discount is null)
            {
                return NotFound();
            }

            return discount;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateDiscount command)
        {
            await _dispatcher.SendAsync(command.BindId(c => c.Id));

            return Accepted();
        }
    }
}