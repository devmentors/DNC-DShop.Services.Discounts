using System;
using DShop.Common.Types;
using DShop.Services.Discounts.Dto;

namespace DShop.Services.Discounts.Queries
{
    public class GetDiscount : IQuery<DiscountDetailsDto>
    {
        public Guid Id { get; set; } 
    }
}