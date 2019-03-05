using System;
using System.Threading.Tasks;
using DShop.Services.Discounts.Domain;
using DShop.Services.Discounts.IntegrationTests.Fixtures;
using DShop.Services.Discounts.Messages.Commands;
using DShop.Services.Discounts.Messages.Events;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Shouldly;
using Xunit;

namespace DShop.Services.Discounts.IntegrationTests
{
    public class DiscountsCommandsTests :  IClassFixture<MongoDbFixture>, IClassFixture<RabbitMqFixture>,
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly MongoDbFixture _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;

        public DiscountsCommandsTests(MongoDbFixture mongoFixture, RabbitMqFixture rabbitFixture,
            WebApplicationFactory<Startup> factory)
        {
            _mongoDbFixture = mongoFixture;
            _rabbitMqFixture = rabbitFixture;
            var client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_Discount_Command_Should_Create_MongoEntity()
        {
            var customerId = Guid.NewGuid();
            await _mongoDbFixture.InsertAsync("Customers", new Customer(customerId, "test@test.com"));
            
            var command = new CreateDiscount(Guid.NewGuid(), customerId, "DISCOUNT", 10);
            var creationTask = await _rabbitMqFixture.SubscribeAndGetAsync<DiscountCreated>(_mongoDbFixture.GetMongoEntity, command.Id);
            await _rabbitMqFixture.PublishAsync(command);
            
            var createdMongoEntity = await creationTask.Task;
            
            createdMongoEntity.Id.ShouldBe(command.Id);
            createdMongoEntity.CustomerId.ShouldBe(command.CustomerId);
            createdMongoEntity.Code.ShouldBe(command.Code);
            createdMongoEntity.Percentage.ShouldBe(command.Percentage);
        }
    }
}