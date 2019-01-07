using System;
using System.Collections.Generic;
using DShop.Common.Types;
using DShop.Services.Discounts.Dto;

namespace DShop.Services.Discounts.Queries
{
    public class FindDiscounts : IQuery<IEnumerable<DiscountDto>>
    {
        public Guid CustomerId { get; set; }
    }
}