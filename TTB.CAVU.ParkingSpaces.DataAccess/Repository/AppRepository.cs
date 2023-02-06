using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.DataAccess.Config;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;

namespace TTB.CAVU.ParkingSpaces.DataAccess.Repository
{
    public class AppRepository : IAppRepository
    {
        private readonly ConnectionSettings _connectionSettings;
        private readonly ConcurrentDictionary<Type, object> Repos = new();
        public AppRepository(IOptions<ConnectionSettings> connectionSettings, ILogger<AppRepository> logger)
        {
            _connectionSettings = connectionSettings.Value;
            var client = new MongoClient(_connectionSettings.ConnectionString);
            var db = client.GetDatabase(_connectionSettings.Database);

            // Added a parking space(s) collection to avoid hardcoding a current value which in production could have changed :)
            CreateRepo<ParkingSpace>(db, "spaces");
            CreateRepo<SpaceBooking>(db, "bookings");
            CreateRepo<Seed>(db, "seeds");
            logger.LogInformation("Initialised MongoDB Repositories");
        }

        // Used this generics technique to simplify the creation and management of Generic Repositories
        // In Hindsight, A simpler solution would surfice, however 😜
        public IGenericRepositoryService<T> GetRepository<T>() where T : AppDataModel
        {
            if (Repos.TryGetValue(typeof(T), out var repo))
            {
                var lazyRepo = (Lazy<GenericRepositoryService<T>>)repo;
                return lazyRepo.Value;
            }
            throw new KeyNotFoundException($"Repository Key not found for type {nameof(T)}");
        }

        private void CreateRepo<T>(IMongoDatabase db, string? name) where T : AppDataModel
        {
            Repos.TryAdd(typeof(T), new Lazy<GenericRepositoryService<T>>(() => new GenericRepositoryService<T>(db.GetCollection<T>(name))));
        }
    }
}
