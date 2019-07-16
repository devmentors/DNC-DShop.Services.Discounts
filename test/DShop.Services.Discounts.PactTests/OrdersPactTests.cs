using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DShop.Services.Discounts.Dto;
using Newtonsoft.Json;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace DShop.Services.Discounts.PactTests
{
    public class OrdersPactTests : IClassFixture<OrderApiMock>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _serviceUri;
        
        public OrdersPactTests(OrderApiMock fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _serviceUri = fixture.ServiceUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public async Task Given_Valid_Order_Id_Order_Should_Be_Returned()
        {
            var orderId = new Guid("c68a24ea-384a-4fdc-99ce-8c9a28feac64");
            
            //ARRANGE 
            _mockProviderService
                .Given("Existing order")
                .UponReceiving("A GET request to retrieve order details")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/orders/{orderId}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new OrderDetailsDto
                    {
                        Id = orderId,
                        ItemsCount = 4,
                        Status = "completed"
                    }
                });

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_serviceUri}/orders/{orderId}");
            var json = await response.Content.ReadAsStringAsync();
            var orderDetails = JsonConvert.DeserializeObject<OrderDetailsDto>(json);
            
            Assert.Equal(orderDetails.Id, orderId);
        }
    }
}