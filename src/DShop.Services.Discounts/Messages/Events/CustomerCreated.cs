using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Discounts.Messages.Events
{
    [MessageNamespace("customers")]
    public class CustomerCreated : IEvent
    {
        public Guid Id { get; }

        [JsonConstructor]
        public CustomerCreated(Guid id)
        {
            Id = id;
        }
    }
}