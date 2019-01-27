using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Common.Types;
using DShop.Services.Discounts.Domain;
using DShop.Services.Discounts.Messages.Commands;
using DShop.Services.Discounts.Messages.Events;
using DShop.Services.Discounts.Repositories;
using Microsoft.Extensions.Logging;

namespace DShop.Services.Discounts.Handlers.Discounts
{
    public class CreateDiscountHandler : ICommandHandler<CreateDiscount>
    {
        private readonly IDiscountsRepository _discountsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<CreateDiscountHandler> _logger;

        public CreateDiscountHandler(IDiscountsRepository discountsRepository,
            ICustomersRepository customersRepository,
            IBusPublisher busPublisher, ILogger<CreateDiscountHandler> logger)
        {
            _discountsRepository = discountsRepository;
            _customersRepository = customersRepository;
            _busPublisher = busPublisher;
            _logger = logger;
        }
        
        // Command -> Event - how to track this simple use case?
        // Command -> Event -> Event -> Command -> Event ... or this one?

        public async Task HandleAsync(CreateDiscount command, ICorrelationContext context)
        {
            // Customer validation
            var customer = await _customersRepository.GetAsync(command.CustomerId);
            if (customer is null)
            {
                //onError: -> publish CreateDiscountRejected
                throw new DShopException("customer_not_found",
                    $"Customer with id: '{command.CustomerId}' was not found.");
                
                // _logger.LogWarning($"Customer with id: '{command.CustomerId}' was not found.");
                // await _busPublisher.PublishAsync(new CreateDiscountRejected(command.CustomerId,
                //     $"Customer with id: '{command.CustomerId}' was not found.",
                //     "customer_not_found"), context);
                
                // return;
            }

            // Unique code validation
            var discount = new Discount(command.Id, command.CustomerId,
                command.Code, command.Percentage);
            await _discountsRepository.AddAsync(discount);
            await _busPublisher.PublishAsync(new DiscountCreated(command.Id,
                command.CustomerId, command.Code, command.Percentage), context);
            // Send an email about a new discount to the customer
        }
    }
}