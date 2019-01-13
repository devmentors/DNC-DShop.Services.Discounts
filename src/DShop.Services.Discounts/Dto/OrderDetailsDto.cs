using System;
using System.Collections.Generic;

namespace DShop.Services.Discounts.Dto
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int ItemsCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public IEnumerable<OrderItemDto> Items { get; set; }
    }
}