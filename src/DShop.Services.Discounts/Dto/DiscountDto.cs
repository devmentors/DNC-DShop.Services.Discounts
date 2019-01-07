using System;

namespace DShop.Services.Discounts.Dto
{
    public class DiscountDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Code { get; set; }
        public double Percentage { get; set; }
        public bool Available { get; set; }
    }
}