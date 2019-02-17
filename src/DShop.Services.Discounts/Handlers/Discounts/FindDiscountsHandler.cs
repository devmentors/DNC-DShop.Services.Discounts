using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.Mongo;
using DShop.Services.Discounts.Domain;
using DShop.Services.Discounts.Dto;
using DShop.Services.Discounts.Metrics;
using DShop.Services.Discounts.Queries;

namespace DShop.Services.Discounts.Handlers.Discounts
{
    public class FindDiscountsHandler : IQueryHandler<FindDiscounts, IEnumerable<DiscountDto>>
    {
        private readonly IMongoRepository<Discount> _discountsRepository;
        private readonly IMetricsRegistry _registry;    

        public FindDiscountsHandler(IMongoRepository<Discount>  discountsRepository, IMetricsRegistry registry)
        {
            _discountsRepository = discountsRepository;
            _registry = registry;
        }

        public async Task<IEnumerable<DiscountDto>> HandleAsync(FindDiscounts query)
        {
            _registry.IncrementFindDiscountsQuery();
            
            var discounts = await _discountsRepository.FindAsync(
                c => c.CustomerId == query.CustomerId);

            return discounts.Select(d => new DiscountDto
            {
                Id = d.Id,
                CustomerId = d.CustomerId,
                Code = d.Code,
                Percentage = d.Percentage,
                Available = !d.UsedAt.HasValue
            });
        }
    }
}