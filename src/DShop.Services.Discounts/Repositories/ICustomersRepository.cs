using System;
using System.Threading.Tasks;
using DShop.Services.Discounts.Domain;

namespace DShop.Services.Discounts.Repositories
{
    public interface ICustomersRepository
    {
        Task<Customer> GetAsync(Guid id);
        Task AddAsync(Customer customer);
    }
}