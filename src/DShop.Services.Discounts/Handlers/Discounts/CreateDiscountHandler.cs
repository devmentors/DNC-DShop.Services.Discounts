using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Services.Discounts.Domain;
using DShop.Services.Discounts.Messages.Commands;
using DShop.Services.Discounts.Repositories;

namespace DShop.Services.Discounts.Handlers.Discounts
{
    public class CreateDiscountHandler : ICommandHandler<CreateDiscount>
    {
        private readonly IDiscountsRepository _discountsRepository;

        public CreateDiscountHandler(IDiscountsRepository discountsRepository)
        {
            _discountsRepository = discountsRepository;
        }

        public async Task HandleAsync(CreateDiscount command, ICorrelationContext context)
        {
            // Customer validation
            // Unique code validation
            
            var discount = new Discount(command.Id, command.CustomerId,
                command.Code, command.Percentage);
            await _discountsRepository.AddAsync(discount);
            
            // Send an email about a new discount to the customer
        }
    }
}