using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ITGA.Common.Mongo
{
    public class MongoSeeder : IDatabaseSeeder
    {
        protected readonly IMongoDatabase Database;
        private readonly ILogger<MongoSeeder> _logger;

        public MongoSeeder(IMongoDatabase database, ILogger<MongoSeeder> logger)
        {
            Database = database;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                var collectionCursor = await Database.ListCollectionsAsync();
                var collections = await collectionCursor.ToListAsync();
                var userCollections = collections.Where(x =>
                    !x.Elements.Single(y => y.Name == "name").Value.AsString.StartsWith("system.")).ToList();
                if (userCollections.Any()) return; // ignore System Collections (starting with system.)
                await CustomSeedAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Fatal error : {e.Message}");
                Environment.Exit(-1);
            }
            
        }

        protected virtual async Task CustomSeedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
