using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DShop.Common;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Discounts.Domain;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using RawRabbit;
using RawRabbit.Enrichers.MessageContext;

namespace DShop.Services.Discounts.IntegrationTests.Fixtures
{
    public class RabbitMqFixture
    {
        private readonly RawRabbit.Instantiation.Disposable.BusClient _client;
        bool _disposed = false;
        
        public RabbitMqFixture()
        {
            _client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions()
            {
                ClientConfiguration = new RawRabbitConfiguration
                {
                    Hostnames = new List<string> { "localhost" },
                    VirtualHost = "/",
                    Port = 5672,
                    Username = "guest",
                    Password = "guest",

                },
                Plugins = p => p  
                    .UseAttributeRouting()
                    .UseRetryLater()
                    .UseMessageContext<CorrelationContext>()
                    .UseContextForwarding()
            });
        }

        public Task PublishAsync<TMessage>(TMessage message) where TMessage : class
            => _client.PublishAsync(message, ctx => 
                ctx.UseMessageContext(CorrelationContext.Empty).UsePublishConfiguration(p => p.WithRoutingKey(GetRoutingKey(@message))));
        
        public async Task<TaskCompletionSource<Discount>> SubscribeAndGetAsync<TEvent>(
            Func<Guid, TaskCompletionSource<Discount>, Task> onMessageReceived, Guid id) where TEvent : IEvent
        {
            var taskCompletionSource = new TaskCompletionSource<Discount>();
            var guid = Guid.NewGuid().ToString();
            
            await _client.SubscribeAsync<TEvent>(
                async _ => await onMessageReceived(id, taskCompletionSource),
                ctx => ctx.UseSubscribeConfiguration(cfg =>
                    cfg
                        .FromDeclaredQueue(
                            builder => builder
                                .WithDurability(false)
                                .WithName(guid))));
            return taskCompletionSource;
        }
        
        private string GetRoutingKey<T>(T message)
        {
            var @namespace = "discounts";
            @namespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{@namespace}{typeof(T).Name.Underscore()}".ToLowerInvariant();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _client.Dispose();
            }

            _disposed = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}