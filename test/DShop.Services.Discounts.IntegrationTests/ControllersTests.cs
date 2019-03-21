using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DShop.Services.Discounts.Domain;
using DShop.Services.Discounts.Dto;
using DShop.Services.Discounts.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RawRabbit.Serialization;
using Shouldly;
using Xunit;

namespace DShop.Services.Discounts.IntegrationTests
{
    public class ControllersTests : IClassFixture<MongoDbFixture>,
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly MongoDbFixture _mongoDbFixture;
        private readonly HttpClient _client;

        public ControllersTests(MongoDbFixture mongoDbFixture,WebApplicationFactory<Startup> factory)
        {
            _mongoDbFixture = mongoDbFixture;
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("")]
        [InlineData("ping")]
        [InlineData("discounts")]
        public async Task Given_Endpoints_Should_Return_Success_Http_Status_Code(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            response.IsSuccessStatusCode.ShouldBeTrue();
        }

        [Fact]
        public async Task Discounts_Details_Endpoint_Should_Return_Correct_Model()
        {
            var customer = new Customer(Guid.NewGuid(), "customer@test.com");
            var discount = new Discount(Guid.NewGuid(), customer.Id, "DISCOUNT", 10);

            await _mongoDbFixture.InsertAsync("Customers", customer);
            await _mongoDbFixture.InsertAsync("Discounts", discount);

            var response = await _client.GetAsync($"discounts/{discount.Id}");
            var contentString = await response.Content.ReadAsStringAsync();
            var discountsDetails = JsonConvert.DeserializeObject<DiscountDetailsDto>(contentString);
            
            discountsDetails.Customer.ShouldNotBeNull();
            discountsDetails.Customer.Id.ShouldBe(customer.Id);
            discountsDetails.Customer.Email.ShouldBe(customer.Email);
            discountsDetails.Discount.ShouldNotBeNull();
            discountsDetails.Discount.Id.ShouldBe(discount.Id);
            discountsDetails.Discount.CustomerId.ShouldBe(customer.Id);
            discountsDetails.Discount.Code.ShouldBe(discount.Code);
            discountsDetails.Discount.Percentage.ShouldBe(discount.Percentage);
        }
    }
}