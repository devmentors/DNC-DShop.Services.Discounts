using System;
using System.Threading.Tasks;
using DShop.Common.Mongo;
using DShop.Services.Discounts.Domain;
using MongoDB.Driver;
using Xunit;

namespace DShop.Services.Discounts.IntegrationTests.Fixtures
{
    public class MongoDbFixture : IDisposable
    {
        private readonly IMongoClient _client;
        private readonly IMongoCollection<Discount> _collection;
        private const string CollectionName = "Discounts";
        private const string DatabaseName = "discounts-service";
        bool _disposed = false;
        
        public MongoDbFixture()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            var database = _client.GetDatabase(DatabaseName);
            
            new MongoDbInitializer(database, null, new MongoDbOptions()).InitializeAsync().GetAwaiter().GetResult();
            
            _collection = database.GetCollection<Discount>(CollectionName);
        }
        
        public async Task GetMongoEntity(Guid expectedId, TaskCompletionSource<Discount> receivedTask)
        {
            if (expectedId == null)
            {
                throw new ArgumentNullException(nameof(expectedId));
            }

            Discount entity = null;
            
            try
            {
                entity = await _collection.Find(d => d.Id == expectedId).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            if (entity is null)
            {
                receivedTask.TrySetCanceled();
                return;
            }
            
            receivedTask.TrySetResult(entity);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                _client.DropDatabase(DatabaseName);
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