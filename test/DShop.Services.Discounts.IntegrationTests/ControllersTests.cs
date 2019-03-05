using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace DShop.Services.Discounts.IntegrationTests
{
    public class ControllersTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ControllersTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("")]
        [InlineData("ping")]
        [InlineData("discounts")]
        public async Task Test(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            response.IsSuccessStatusCode.ShouldBeTrue();
        }
    }
}