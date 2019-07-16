using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace DShop.Services.Discounts.PactTests
{
    public class OrderApiMock : IDisposable
    {
        private readonly IPactBuilder _pactBuilder;

        private readonly int _servicePort = 9222;
        public IMockProviderService MockProviderService { get; }

        public string ServiceUri => $"http://localhost:{_servicePort}";

        public OrderApiMock()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\..\..\..\pacts",
                LogDir = @".\pact_logs"
            };

            _pactBuilder = new PactBuilder(pactConfig)
                .ServiceConsumer("discounts")
                .HasPactWith("Orders");

            MockProviderService = _pactBuilder.MockService(_servicePort, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _pactBuilder.Build();
                }

                _disposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
    }
}