using System.Threading.Tasks;
using DShop.Common.Mongo;
using DShop.Services.Discounts.Domain;

namespace DShop.Services.Discounts.Repositories
{
    public class DiscountsRepository : IDiscountsRepository
    {
        private readonly IMongoRepository<Discount> _repository;

        public DiscountsRepository(IMongoRepository<Discount> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Discount discount)
            => await _repository.AddAsync(discount);

        public async Task UpdateAsync(Discount discount)
            => await _repository.UpdateAsync(discount);
    }
}