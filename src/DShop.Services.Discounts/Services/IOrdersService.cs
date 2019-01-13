using System;
using System.Threading.Tasks;
using DShop.Services.Discounts.Dto;
using RestEase;

namespace DShop.Services.Discounts.Services
{
    public interface IOrdersService
    {
        [AllowAnyStatusCode]
        [Get("orders/{id}")]
        Task<OrderDetailsDto> GetAsync([Path] Guid id);
    }
}