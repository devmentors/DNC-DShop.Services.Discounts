using System;
using System.Threading.Tasks;
using DShop.Common.Mongo;
using DShop.Common.Types;
using DShop.Services.Discounts.Domain;
using MongoDB.Driver;
using Xunit;

namespace DShop.Services.Discounts.IntegrationTests.Fixtures
{
    public class MongoDbFixture : IDisposable
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Discount> _collection;
        private const string CollectionName = "Discounts";
        private const string DatabaseName = "discounts-service";
        bool _disposed = false;
        
        public MongoDbFixture()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase(DatabaseName);
            InitializeMongo();
            _collection = _database.GetCollection<Discount>(CollectionName);
        }
        
        public void InitializeMongo()
            => new MongoDbInitializer(_database, null, new MongoDbOptions())
                .InitializeAsync().GetAwaiter().GetResult();

        public Task InsertAsync<TEntity>(string collectionName, TEntity entity) where TEntity : IIdentifiable
            => _database.GetCollection<TEntity>(collectionName).InsertOneAsync(entity);
        
        public async Task GetMongoEntity(Guid expectedId, TaskCompletionSource<Discount> receivedTask)
        {
            if (expectedId == null)
            {
                throw new ArgumentNullException(nameof(expectedId));
            }

            var entity = await _collection.Find(d => d.Id == expectedId).SingleOrDefaultAsync();
            
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